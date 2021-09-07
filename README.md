# Todo
Demo project for todo task application build with asp.net core and vue.js.

## Description

This demo project implements a todo application that allows for creating, editing and deleting todo tasks. Todo tasks can also contain subtasks and subtasks can contain their own subtasks. There is no limit to how deep this hierarchy can extend to, but the UI will at some point break. The UI will fetch children from the API on demand when drilling down to grand children and beyond of the main tasks. 

The backend uses MySQL database and entity framework to communicate with it.

The main components of the solution are
- **Todo.Frontend** Vue application for frontend
- **Todo.Backend** ASP.NET Core application that implements the API for the tasks and handles database transactions with MySQL database.

## Todo.Frontend

Main vue components for this application are:

- **Tasks.vue** Task list view of the application that lists the main tasks and allows sorting and filtering.
- **Task.vue** Individual task within the task and subtask list.
- **TaskEdtior.vue** Editor component for tasks. Can be rendered as modal or as embedded component.

UI uses vue bootstrap package and communication between application and api is using axios. Tasks use [vuedraggable](https://github.com/SortableJS/Vue.Draggable) for drag and drop functionality.

## Todo.Backend

Simple ASP.NET API project that has restful api for tasks. The project uses swagger to generate api discovery document and user interface for testing it. Available methods are
- GET /api/tasks - returns base level tasks and their children
- POST /api/tasks - add new task
- GET /api/tasks/{} - get task with id and their children
- PUT /api/tasks/{id} - edit existing task 
- DELETE /api/tasks/{id} - delete task

## Demo ##

[Link to demo](http://todovue.s3-website.eu-central-1.amazonaws.com/)

## Setup

1. Build and run the Todo.Backend project with dotnet commands or inside compatibile IDE. 
2. Inside src/Todo.Frontend/ run npm install and npm run serve
3. Navigate to http://localhost:5002


