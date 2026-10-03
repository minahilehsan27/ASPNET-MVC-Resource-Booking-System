# ASP.NET MVC Resource Booking System

A role-based web app where employees request to book resources (such as rooms or devices) and an admin approves the requests. Booking requests and approvals update on the page without a reload, using jQuery AJAX.

## Features

- Two roles, **Admin** and **Employee**, using ASP.NET Identity
- **Admin only:** add new resources through a validated form, and approve booking requests
- **Admin and Employee:** view the resource list
- **Employee only:** a "Request Booking" button that changes to "Requested" without reloading the page
- The server blocks duplicate requests for the same resource by the same employee
- Admin approval dashboard that updates dynamically
- Unauthorized users are redirected to an Access Denied page

## Architecture

The app follows the **MVC (Model-View-Controller)** pattern:

| Part | Role |
|---|---|
| Models | `Resource` and `Booking` classes with validation rules |
| Views | Razor pages, including partial views for the approval dashboard |
| Controllers | `AccountController` and `BookingController` handle requests and authorization |
| Repository | `BookingRepository` handles saving and loading bookings |

## Concepts Used

- **Role-based access control:** `[Authorize(Roles = "...")]` attributes protect pages and AJAX endpoints
- **jQuery AJAX:** requests and approvals are sent to the server in the background, and the page updates without a refresh
- **Partial views:** the approval dashboard reloads only the part of the page that changed
- **Model validation:** required fields are checked on the form and on the server
- **Audit trail:** each resource stores the ID of the admin who created it

## Built With

- C#
- ASP.NET MVC
- ASP.NET Identity
- jQuery AJAX

## About

Made for the Web Technologies course (SEF23), Homework 08, at the University of the Punjab.
