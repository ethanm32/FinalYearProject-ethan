// @ts-check
"use strict";
var EOL = require('os').EOL;
var fs = require('fs');
var path = require('path');
// Choose 'tap' rather than 'min' or 'xunit'. The reason is that
// 'min' produces undisplayable text to stdout and stderr under piped/redirect, 
// and 'xunit' does not print the stack trace from the test.
var defaultMochaOptions = { ui: 'tdd', reporter: 'tap', timeout: 2000 };

function reset_result() {
    return {
        'title': '',
        'passed': false,
        'pending': false,
        'stdOut': '',
        'stdErr': ''
    };
}

var result = reset_result();

var find_tests = function (testFileList, discoverResultFile, projectFolder) {
    var Mocha = detectMocha(projectFolder);
    if (!Mocha) {
        return;
    }

    function getTestList(suite, testFile) {
        if (suite) {
            if (suite.tests && suite.tests.length !== 0) {
                suite.tests.forEach(function (t, i, testArray) {
                    testList.push({
                        test: t.fullTitle(),
                        suite: suite.fullTitle(),
                        file: testFile,
                        line: 0,
                        column: 0
                    });
                });
            }

            if (suite.suites) {
                suite.suites.forEach(function (s, i, suiteArray) {
                    getTestList(s, testFile);
                });
            }
        }
    }
    var testList = [];
    testFileList.split(';').forEach(function (testFile) {
        var mocha = initializeMocha(Mocha, projectFolder);
        process.chdir(path.dirname(testFile));

        try {
            mocha.addFile(testFile);
            mocha.loadFiles();
            getTestList(mocha.suite, testFile);
        } catch (e) {
            //we would like continue discover other files, so swallow, log and continue;
            console.error("Test discovery error:", e, "in", testFile);
        }
    });

    var fd = fs.openSync(discoverResultFile, 'w');
    fs.writeSync(fd, JSON.stringify(testList));
    fs.closeSync(fd);
};
module.exports.find_tests = find_tests;

var run_tests = function (testCases, callback) {
    function post(event) {
        callback(event);
        hook_outputs();
    }

    function escapeRegExp(string) {
        return string.replace(/[.*+?^${}()|[\]\\]/g, '\\$&'); // $& means the whole matched string
    }

    function append_stdout(string, encoding, fd) {
        result.stdOut += string;
        return true;
    }
    function append_stderr(string, encoding, fd) {
        result.stdErr += string;
        return true;
    }
    function hook_outputs() {
        process.stdout.write = append_stdout;
        process.stderr.write = append_stderr;
    }

    hook_outputs();

    var testResults = [];
    var Mocha = detectMocha(testCases[0].projectFolder);
    if (!Mocha) {
        return;
    }

    var mocha = initializeMocha(Mocha, testCases[0].projectFolder);

    var testGrepString = '^(' + testCases.map(function (testCase) {
        return escapeRegExp(testCase.testName);
    }).join('|') + ')$';

    if (testGrepString) {
        mocha.grep(new RegExp(testGrepString));
    }
    mocha.addFile(testCases[0].testFile);

    var runner = mocha.run(function (code) {
        process.exitCode = code ? code : 0;
    });

    // See events available at https://github.com/mochajs/mocha/blob/8cae7a34f0b6eafeb16567beb8852b827cc5956b/lib/runner.js#L47-L57
    runner.on('suite', function (suite) {
        post({
            type: 'suite start',
            result: result
        });
    });

    runner.on('suite end', function (suite) {
        post({
            type: 'suite end',
            result: result
        });
    });

    runner.on('hook', function (hook) {
        post({
            type: 'hook start',
            title: hook.title,
            result: result
        });
    });

    runner.on('hook end', function (hook) {
        post({
            type: 'hook end',
            title: hook.title,
            result: result
        });
    });

    runner.on('start', function () {
        post({
            type: 'start',
            result: result
        });
    });

    runner.on('pending', function (test) {
        result.pending = true;
        result.title = test.fullTitle();
        post({
            type: 'pending',
            title: result.title,
            result: result
        });
        result = reset_result();
    });

    runner.on('test', function (test) {
        result.title = test.fullTitle();
        post({
            type: 'test start',
            title: result.title
        });
    });

    runner.on('end', function () {
        post({
            type: 'end',
            result: result
        });
    });

    runner.on('pass', function (test) {
        result.passed = true;
        post({
            type: 'result',
            title: result.title,
            result: result
        });
        result = reset_result();
    });

    runner.on('fail', function (test, err) {
        result.passed = false;
        post({
            type: 'result',
            title: result.title,
            result: result
        });
        result = reset_result();
    });
};

function logError() {
    var errorArgs = Array.prototype.slice.call(arguments);
    errorArgs.unshift("NTVS_ERROR:");
    console.error.apply(console, errorArgs);
}

function detectMocha(projectFolder) {
    try {
        var node_modulesFolder = projectFolder;
        var mochaJsonPath = path.join(node_modulesFolder, 'test', 'mocha.json');
        if (fs.existsSync(mochaJsonPath)) {
            var opt = require(mochaJsonPath);
            if (opt && opt.path) {
                node_modulesFolder = path.resolve(projectFolder, opt.path);
            }
        }

        var mochaPath = path.join(node_modulesFolder, 'node_modules', 'mocha');
        var Mocha = new require(mochaPath);
        return Mocha;
    } catch (ex) {
        logError(
            'Failed to find Mocha package.  Mocha must be installed in the project locally.' + EOL +
            'Install Mocha locally using the npm manager via solution explorer' + EOL +
            'or with ".npm install mocha --save-dev" via the Node.js interactive window.');
        return null;
    }
}

function initializeMocha(Mocha, projectFolder) {
    var mocha = new Mocha();
    applyMochaOptions(mocha, getMochaOptions(projectFolder));
    return mocha;
}

function applyMochaOptions(mocha, options) {
    if (options) {
        for (var opt in options) {
            var mochaOpt = mocha[opt];
            var optValue = options[opt];

            if (typeof mochaOpt === 'function') {
                try {
                    mochaOpt.call(mocha, optValue);
                } catch (e) {
                    console.log("Could not set mocha option '" + opt + "' with value '" + optValue + "' due to error:", e);
                }
            }
        }
    }
}

function getMochaOptions(projectFolder) {
    var mochaOptions = defaultMochaOptions;
    try {
        var optionsPath = path.join(projectFolder, 'test', 'mocha.json');
        var options = require(optionsPath) || {};
        for (var opt in options) {
            mochaOptions[opt] = options[opt];
        }
        console.log("Found mocha.json file. Using Mocha settings: ", mochaOptions);
    } catch (ex) {
        console.log("Using default Mocha settings");
    }

    // set timeout to 10 minutes, because the default of 2 sec is too short for debugging scenarios
    if (typeof (v8debug) === 'object') {
        mochaOptions['timeout'] = 600000;
    }

    return mochaOptions;
}

module.exports.run_tests = run_tests;