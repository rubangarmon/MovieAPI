# MovieAPI

Running fake server
```
json-server --watch db.json --routes routes.json
```
## Setup
### Adding user-secrets
```powershell
$token = ""
dotnet user-secrets set "HttpMovieRepository:Token" "$token"
```