build:
	dotnet build SlimeBattleSystem/*.csproj --configuration Debug

release:
	dotnet build SlimeBattleSystem/*.csproj --configuration Release

test:
	dotnet test SlimeBattleSystem.Tests/*.csproj

clean:
	git clean -xdf
