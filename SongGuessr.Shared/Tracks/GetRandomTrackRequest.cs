﻿using SongGuessr.Shared.Playlists;

namespace SongGuessr.Shared.Tracks;

public record GetRandomTrackRequest(List<Track> PlaylistTracks);
