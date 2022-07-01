build:
	dotnet build PokerSharp/*.csproj

test:
	dotnet test PokerSharp.Tests/*.csproj

clean:
	git clean -xdf
