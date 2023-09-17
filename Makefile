.PHONY: migrate
migrate:
	dotnet ef migrations add $(name)
	dotnet ef database update

.PHONY: run
run:
	dotnet run
