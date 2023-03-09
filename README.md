# CommandService
Service for console commands' management for various platforms.

## Table of contents
* [Introduction](#introduction)
* [Used technologies](#used-technologies)
* [Features](#features)
* [Project status](#project-status)

## Introduction
This project is a part of a bigger microservices architecture designed to help people remember console commands for different platforms. It focues on adding, editing, 
etc. of console commands for different platforms managed by the platforms service (https://github.com/Kprzyby/PlatformService). It also consumes Azure Service Bus messages
published by that service to always have valid information about the platforms. The project was deployed locally in Kubernetes
(deployment files here - https://github.com/Kprzyby/PlatformAppK8S).

## Used technologies
* .NET 6.0
* C#
* Swagger
* Azure Service Bus
* Entity Framework Core 7.0.3
* Docker

## Features
* Consuming service bus messages concerning events related to the platform service
* Adding commands for a specified platform
* Editing commands
* Deleting commands
* Loading a singular command for a specified platform
* Loading all commands for a specified platform (pagination, filtering and sorting included)
* Automated migrations

## Project status
The project is still in development. In the future I plan on adding authentication, authorization and better error handling.
