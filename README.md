# Test Project

## Backend

### How to run it locally:
1) Install the [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1)
2) Install `MS SQL`
3) Set  the connection string in `Api/appsettings.Development.json`
4) Try to run the application. It should run all the migrations and seeds automatically

### How to generate a new migration to synchronize the DB structure with application models (from DB.Data directory):
`dotnet ef migrations add Initial --startup-project ../API`
### Apply all the pending migrations (If you don't want the application to apply them automatically on start):
`dotnet ef database update --startup-project ../API`

## Frontend

### How to run it locally:
1) Install the NodeJS 14+ (If nvm is available for your OS, it's strongly recommended to use it instead of the default NodeJS installer)
2) Install [Angular CLI](https://cli.angular.io/) globally
3) Open the command line on `frontend` folder, and install all the node modules with `npm install`
4) Run `ng serve` in the same folder
5) Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.
6) Set the API link rnv variable, if needed

### Layout system:

Project uses 'covalent' grid system as an utilities pack over the CSS flexbox. 
To get more details, check the `Flexbox Layout` section of the [Covalent Documentation](https://teradata.github.io/covalent/v2/#/layouts) or [Material Angular JS docs](https://material.angularjs.org/latest/layout/introduction)

### Code scaffolding:

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

### Further help:

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI README](https://github.com/angular/angular-cli/blob/master/README.md).
