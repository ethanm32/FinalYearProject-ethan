fetch('https://raw.githubusercontent.com/KoreanThinker/billboard-json/main/billboard-hot-100/recent.json')
    .then(response => response.json())
    .then(data => {
        console.log(data)
        console.log(data.data.length)

        addData(data);

    });


function addData(data) {
    var returned = data.data;
    var songs = document.getElementById("songs");
    for (let i = 0; i < 10; i++) {
        var div = document.createElement("div");
        var img = document.createElement("img");
        div.id = "textDiv";
        div.style.color = "white";
        img.src = returned[i].image;

        div.appendChild(img);
        div.innerHTML += '<br>' + '<br>' + '<br>' + 'Name:' + returned[i].name + '<br>' + 'Artist:' + returned[i].artist + '<br>' + '<br>';
        songs.appendChild(div);


        div.onclick = function () {
            var value = this.innerHTML;
            localStorage.setItem("storageName", value);
            window.open('@Url.Action("Song", "User")');


        }
    }
    
}
