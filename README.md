# Todo
Demo project for todo task application build with asp.net core and vue.js.

## Description

This demo project implements a todo application that allows for creating, editing and deleting todo tasks. Todo tasks can also contain subtasks and subtasks can contain their own subtasks. There is no limit to how deep this hierarchy can extend to, but the UI will at some point break.

The backend uses MySQL database and entity framework to communicate with it.

The main components of the solution are
- **Todo.Frontend** Vue application for frontend
- **Todo.Backend** ASP.NET Core application that implements the API for the tasks and handles database transactions with MySQL database.

## Demo ##

[Link to demo](http://todovue.s3-website.eu-central-1.amazonaws.com/)

## Setup

1. Build and run the Todo.Backend project with dotnet commands or inside compatibile IDE. 
2. Inside src/Todo.Frontend/ run npm install and npm run serve
3. Navigate to http://localhost:5002


