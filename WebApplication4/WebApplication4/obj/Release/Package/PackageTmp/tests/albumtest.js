test('fetches album from Spotify API and ensures a response', async () => {
    const mockAlbum = { albums: { items: [{ name: 'Album 1' }, { name: 'Album 2' }] } };
    global.fetch = jest.fn().mockImplementation(() => Promise.resolve({
        json: () => Promise.resolve(mockAlbum)
    }));

    const returnedAlbums = await fetch('https://api.spotify.com/v1/search?q=testalbum&type=album');
    const mockData = await returnedAlbums.json();

    expect(global.fetch).toHaveBeenCalledTimes(1);
    expect(global.fetch).toHaveBeenCalledWith('https://api.spotify.com/v1/search?q=testalbum&type=album');
    expect(mockData.albums.items.length).toEqual(2);
});