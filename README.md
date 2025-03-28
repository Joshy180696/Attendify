# Attendify 🎉

Welcome to **Attendify**, a small full-stack event management application designed to help users create, manage, and track events with ease. This project is built using **ASP.NET Core** for the backend and frontend, with a modern, responsive UI powered by Razor Pages and Bootstrap.

Attendify is actively in development, featuring event creation, RSVP tracking, and a responsive table with pagination and sorting. Future updates may introduce user authentication, event categories, and more interactive features!

---

## 🚀 Project Overview

Attendify aims to simplify event management for users by providing a seamless experience for organizing and tracking events. Key goals include:

- ✅ **Event Creation & Management** – Create and view events with details like title, description, date, and location.
- ✅ **RSVP Tracking** – Allow attendees to RSVP to events and view RSVP details.
- ✅ **Responsive Event Table** – A paginated, sortable table for browsing events.
- ✅ **Modern UI/UX** – Clean, responsive design with Bootstrap for a great user experience.

This README provides an overview of the project, its technologies, architecture, current features, and future plans.

---

## 🛠 Technologies Used

### Full-Stack (ASP.NET Core)
- ⚙ **ASP.NET Core** – Powers both the backend API and frontend Razor Pages for a unified full-stack experience.
- 📄 **Razor Pages** – Handles the frontend UI with server-side rendering for dynamic content.
- 🎨 **Bootstrap** – Provides a responsive, modern UI with components like tables, modals, and buttons.
- 💾 **Entity Framework Core** – Manages database interactions with Neon PostgreSQL.
- 🔗 **Npgsql** – PostgreSQL provider for Entity Framework Core.

### Development Tools
- 💻 **Visual Studio** – Primary IDE for coding and debugging.
- 🔄 **Git** – Version control for tracking changes.
- 🔧 **.NET 9** – Runtime and SDK for building the app.

### ☁ Database & Hosting
- 🛢 **Neon** – Serverless PostgreSQL database for storing event and RSVP data.
- 🚀 **Render** – Free-tier cloud hosting platform for deploying the app, offering:
  - Automatic scaling for traffic spikes.
  - Free SSL for secure connections.
  - Custom domain support at zero cost.

---

## 🏗 Architecture

Attendify follows an **N-layer architecture** to ensure modularity, scalability, and maintainability:

1. **Presentation Layer** (`Attendify.UILayer`)
   - Built with ASP.NET Core Razor Pages and Bootstrap.
   - Handles user interactions (event creation, RSVP submission, event browsing).
   - Communicates with the Business Logic Layer (BLL) via dependency injection.

2. **Business Logic Layer (BLL)** (`Attendify.DomainLayer`)
   - Manages event and RSVP logic through services (e.g., `EventsService`).
   - Interacts with the Data Access Layer (DAL) through interfaces (e.g., `IEventService`, `IUnitOfWork`).

3. **Data Access Layer (DAL)** (`Attendify.DataLayer`)
   - Uses Entity Framework Core with Npgsql to interact with Neon PostgreSQL for data storage and retrieval.
   - Implements the Repository Pattern and Unit of Work for clean data operations.

---

## 🔥 Features

### ✅ Current Features
- **📅 Event Management**
  - Create events with details like title, description, date, time, and location.
  - View a list of events on the Home page with a card-based layout.
  - Browse all events in a responsive table with pagination and sorting (Events History page).

- **📋 RSVP Tracking**
  - Submit RSVPs for events with attendee details (name, email, etc.).
  - View RSVP details for each event on the Event Details page.

- **📊 Events History Table**
  - Paginated table with sorting by columns (e.g., date, title).
  - Responsive design for all screen sizes.

- **🎨 UI/UX**
  - Modern design with Bootstrap for a clean, professional look.
  - Interactive modals for creating events and submitting RSVPs.
  - Loading spinners for async operations (e.g., fetching events).

### ⏳ Planned Features
- 🔐 **User Authentication** – Add login and registration with JWT-based authentication.
- 🏷 **Event Categories** – Organize events by categories (e.g., Social, Professional, Charity).
- 📅 **Calendar View** – Display events in a calendar interface.
- 🔔 **Notifications** – Email reminders for upcoming events and RSVP confirmations.

---

## 🌍 Live Demo

Try Attendify now!

- **Live App** – 🔗 [attendify-3goc.onrender.com](https://attendify-3goc.onrender.com/)

### Demo Events
The app is pre-populated with events for testing:
- **Run for a Cause 5K** – March 27, 2025
- **Spring Fling Picnic 2025** – March 27, 2025
- **Tech Conference 2025** – April 15, 2025

> **Note**: The app is hosted on Render's free tier, which may cause delays in requests due to cold starts when the server is idle. Please allow a few seconds for responses.

