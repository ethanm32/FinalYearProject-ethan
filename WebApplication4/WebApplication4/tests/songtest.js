test('fetches song from Spotify API and ensures a response', async () => {
    const mockSongs = { tracks: { items: [{ name: 'Song 1' }, { name: 'Song 2' }] } };
    global.fetch = jest.fn().mockImplementation(() => Promise.resolve({
        json: () => Promise.resolve(mockSongs)
    }));

    //returns the track in a fetch such as in the code
    const returnedTracks = await fetch('https://api.spotify.com/v1/search?q=testtrack&type=track');
    const songData = await returnedTracks.json();

    expect(global.fetch).toHaveBeenCalledTimes(1);
    expect(global.fetch).toHaveBeenCalledWith('https://api.spotify.com/v1/search?q=testtrack&type=track');
    //checks that two api calls have happened and populated data
    expect(songData.tracks.items.length).toEqual(2);
});