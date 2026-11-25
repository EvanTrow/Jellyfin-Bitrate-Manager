# Jellyfin Bitrate Manager Plugin

This plugin adds a dashboard page that lists every user and lets you view and edit their `Internet streaming bitrate limit (Mbps)` in one place, instead of opening each user's settings.

It edits Jellyfin's own per-user setting, so limits stay in effect if the plugin is removed, and changes made in `Dashboard` -> `Users` show up on the page too.

Plugin repository manifest URL:

- https://raw.githubusercontent.com/EvanTrow/Jellyfin-Bitrate-Manager/main/manifest.json

## Requirements

- Jellyfin 12.4.x

## 1. Add the Plugin Repository in Jellyfin

1. Open Jellyfin Admin Dashboard.
2. Go to `Plugins` -> `Manage Repositories`.
3. Click `+ New Repository` to add a repository.
4. Use:
    - Name: `Bitrate Manager` (or any name you prefer)
    - URL: `https://raw.githubusercontent.com/EvanTrow/Jellyfin-Bitrate-Manager/main/manifest.json`
5. Click `Add`.
6. Refresh the page and go back to `Plugins` and install `Bitrate Manager`.
7. Restart Jellyfin.

## 2. Edit Limits

1. Open Jellyfin Admin Dashboard -> `Bitrate Manager` (in the sidebar, under `Plugins`).
2. Enter a limit in Mbps next to each user. Decimals such as `4.5` are allowed; the maximum is 2147.
3. Leave a limit blank (or enter `0`) to use the server default shown at the top of the page.
4. Click `Save`. Changed rows are highlighted until saved, and only those users are updated.

The limit is per stream and only applies to devices outside your local network.

## API

Both endpoints require an administrator. Limits are in bits per second, and `0` means the server default applies.

| Method | Path | Description |
| --- | --- | --- |
| `GET` | `/BitrateManager/Users` | Lists all users with their limits, plus the server default. |
| `POST` | `/BitrateManager/Users` | Updates the given users. Body: `[{ "UserId": "...", "RemoteClientBitrateLimit": 8000000 }]` |

## Troubleshooting

- `Bitrate Manager` isn't in the sidebar:
    - Restart Jellyfin after installing, then refresh the browser.
- A limit isn't applied during playback:
    - Limits only apply to connections Jellyfin considers remote. Check `LAN Networks` and `Known Proxies` in `Dashboard` -> `Networking`.
- Check Jellyfin logs for `BitrateManager` errors.

## Development

- [Contributing guidelines](CONTRIBUTING.md): dev setup, versioning, pull request requirements, and AI use disclosure.
- [GitHub Actions workflows](docs/WORKFLOWS.md): how version checks, releases, and manifest updates are automated.
