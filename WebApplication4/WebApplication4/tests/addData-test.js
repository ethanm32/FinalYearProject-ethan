

test('Fetch data from API and check data length', async () => {
    const data = {
        data: [
            {
                title: "song 1",
                artist: "artist 1"
            },
            {
                title: "song 2",
                artist: "artist 2"
            }
        ]
    };

    expect(data.data.length).toBeGreaterThan(0);
});

