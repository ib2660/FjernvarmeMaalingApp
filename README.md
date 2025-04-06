# FjernvarmeMaalingApp

## Dansk

### Beskrivelse
FjernvarmeMaalingApp er en Blazor Server webapplikation udviklet som en del af et eksamensprojekt i Avanceret Programmering på Akademi-niveau. Applikationen er designet til at håndtere brugernes målinger af energiforbrug for Jullerup Færgeby Fjernvarmeværk.

### Formål
Formålet med applikationen er at give brugerne mulighed for at registrere og se deres energiforbrug på en brugervenlig måde. Applikationen er bygget med fokus på realtidsopdateringer og en klar adskillelse af ansvarsområder gennem anvendelse af MVVM (Model-View-ViewModel) designmønsteret.

### Overordnet Arkitektur
- **View Lag**: Indeholder Blazor-komponenter og sider, der udgør brugergrænsefladen.
- **ViewModel Lag**: Håndterer applikationens logik og data binding mellem view og viewmodel.
- **Model Lag**: Indeholder dataobjekter og forretningslogik.
- **Service Lag**: Indeholder services, der benyttes af viewmodellaget.
- **Data Lag**: Indeholder modeller for repositories og administrerer logik til interaktion med datakilder.

### Designmønstre
- **Strategi Mønster**: Anvendt til at håndtere forskellige måder at vise og registrere brugermålinger på.
- **Registry Mønster**: Centraliserer adgangen til services gennem Blazor Dependency Injection.
- **Factory Mønster**: Returnerer forskellige typer af forbrugsenheder.
- **Singleton Mønster**: Sikrer, at visse klasser kun har en enkelt instans i applikationen.
- **CQRS Mønster**: Adskiller læse- og skriveoperationer i forskellige interfaces.

### Sikkerhed
- **AuthenticationStateProvider**: Håndterer brugeridentitet og sikrer, at data kun er tilgængelige for autoriserede brugere.
- **Cryptography**: Anvendes til sikker håndtering af brugeradgangskoder gennem kryptering og hashing.

### Videre Udvikling
- **Sikring af Data**: Flytning af dataregistrering til en database server for højere sikkerhed.
- **Oprettelse af API**: Mulighed for at brugere kan hente og uploade data gennem et API.

## English

### Description
FjernvarmeMaalingApp is a Blazor Server web application developed as part of an exam project in Advanced Programming at the Academy level. The application is designed to manage users' energy consumption measurements for Jullerup Færgeby District Heating Plant.

### Purpose
The purpose of the application is to provide users with a user-friendly way to register and view their energy consumption. The application is built with a focus on real-time updates and a clear separation of concerns through the use of the MVVM (Model-View-ViewModel) design pattern.

### Overall Architecture
- **View Layer**: Contains Blazor components and pages that make up the user interface.
- **ViewModel Layer**: Handles the application's logic and data binding between the view and viewmodel.
- **Model Layer**: Contains data objects and business logic.
- **Service Layer**: Contains services used by the viewmodel layer.
- **Data Layer**: Contains models for repositories and manages logic for interacting with data sources.

### Design Patterns
- **Strategy Pattern**: Used to handle different ways of displaying and registering user measurements.
- **Registry Pattern**: Centralizes access to services through Blazor Dependency Injection.
- **Factory Pattern**: Returns different types of consumption units.
- **Singleton Pattern**: Ensures that certain classes have only one instance in the application.
- **CQRS Pattern**: Separates read and write operations into different interfaces.

### Security
- **AuthenticationStateProvider**: Manages user identity and ensures that data is only accessible to authorized users.
- **Cryptography**: Used for secure handling of user passwords through encryption and hashing.

### Further Development
- **Data Security**: Moving data registration to a database server for higher security.
- **API Creation**: Allowing users to fetch and upload data through an API.
