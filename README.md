## This repo contains example projects for using the **Prise Plugin Framework**

<img src="https://github.com/merken/Prise/blob/master/docs/prise.png?raw=true" 
alt="Prise Logo" width="65" height="65" style="float:left; padding-right:15px;" />

Prise is a plugin framework for .NET Core applications, written in .NET Core.
The goal of Prise, is enable you to write **decoupled pieces** of code using the least amount of effort, whilst maximizing the customizability. Prise helps you load plugins from **foreign assemblies**. It is built to decouple the **local** and **remote** dependencies, and strives to avoid assembly mismatches.

List of examples in this repository:
- Assembly Scanning using Prise.AssemblyScanning.Discovery
- An Avalonia cross-platform application with plugins
- An Azure Functions example
- An ASP.NET Core microservice example using multiple plugins
- An example loading a plugin from over the network