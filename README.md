
# Task Management Application

## Overview
The **Task Management Application** is a role-based system built to efficiently manage projects, tasks, and developer requests.  
It provides a clear separation of responsibilities between **Managers** and **Developers**, enabling smooth project tracking and collaboration.

---

## Roles and Responsibilities

### Manager
The **Manager** oversees all projects and tasks in the system.  
They are responsible for project creation, task assignment, and progress tracking.

#### Manager Capabilities:
- **Create Projects** – Initialize new projects.
- **Assign Tasks** – Create and assign tasks to developers.
- **Perform CRUD Operations** on:
  - Projects
  - Tasks
- **Track Project Progress** – Monitor task completion percentages.
- **Review Developer Requests** – Approve or reject task creation requests submitted by developers.

---

### Developer
The **Developer** works on tasks assigned by the Manager and can request new tasks if needed.

#### Developer Capabilities:
- **View Assigned Tasks** – See tasks assigned by the Manager.
- **Update Task Status** – Mark tasks as completed or update progress.
- **Request New Tasks** – Submit a task request for a specific project.
  - The request goes to the **Manager**, who can either **approve** or **reject** it.

---


