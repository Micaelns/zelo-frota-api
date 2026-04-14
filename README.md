# zelo-frota
POC de uma API REST para gestão de frota de caminhões


# Infra Estrutura

- Criar migrations
	
		cd src/Infra
		dotnet ef migrations add InicialMigration --project Infra.csproj --startup-project ../Api/Api.csproj
	* Nome migration: InicialMigration

- Desfazer ultima migration

		cd src/Infra
		dotnet ef migrations remove --project Infra.csproj --startup-project ../Api/Api.csproj
		
- Rodar migrations pendentes

		cd src/Infra
		dotnet-ef database update --project Infra.csproj --startup-project ../Api/Api.csproj

