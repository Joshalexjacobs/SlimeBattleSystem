build:
	dotnet build SlimeBattleSystem/*.csproj

test:
	dotnet test SlimeBattleSystem.Tests/*.csproj

clean:
	git clean -xdf
