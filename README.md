# RAPID RESCUE - E-Ambulance Web Application

![image](https://github.com/user-attachments/assets/2e02cb55-7ae2-4ece-bf21-9b550fd63d81)

Hosted Link: bitrebels.aptechgarden.com

## Project Overview

**RAPID RESCUE** is a highly interactive, responsive, and user-friendly web application designed to help users request emergency ambulance services, track their ambulance in real-time, and manage their medical profiles. This system also supports admins and drivers (EMTs) by providing a comprehensive dispatch system and patient management. The app is designed to work efficiently across all devices, ensuring accessibility and ease of use for everyone.

### Key Features

- 🏥 **Emergency Request**: Easily request an ambulance with options for emergency and non-emergency services.
- 📍 **Real-Time Ambulance Tracking**: Track the dispatched ambulance’s live location and estimated arrival time.
- 📝 **Medical Profile Management**: Users can input, view, and update their medical history, including allergies and emergency contacts.
- 🚑 **Ambulance Dispatch Management**: Admins can assign ambulances to users and track their status in real-time.
- 💬 **Instant Feedback**: Users can provide feedback after receiving services.
- 🆘 **First-Aid Instructions**: Access basic first-aid tips while waiting for emergency help.

---

## Technologies Used

- **ASP.NET Core MVC (C#)**
- **HTML5, CSS3, Bootstrap**
- **JavaScript, jQuery**
- **Google Maps API** for real-time tracking
- **FontAwesome/Bootstrap Icons** for a visually appealing UI

---

## Team Members

- **Syed Kashan Abbas Naqvi**
- **Zayaan Zubair**
- **Prem Kumar**
- **Ahsan Hussain**


---

## Scope of the Project

The **RAPID RESCUE** application provides a seamless, interactive experience for users, admins, and drivers (EMTs):

### For Users:
- 🏠 **Home Page**: Quick access to emergency services and important information.
- 🔑 **User Registration & Login**: Create accounts securely and log in to access features.
- 📋 **Profile Management**: Modify personal details and change passwords.
- 🚨 **Emergency Request**: Book an ambulance by providing required information (hospital, address, contact details).
- 📡 **Real-Time Ambulance Tracking**: View live ambulance location and route from pickup to destination.
- 🩺 **Medical Profile**: Store and manage medical history for quick access during emergencies.
- ❓ **First-Aid Tips**: Get essential first-aid guidance for immediate care.
- 📝 **Feedback Submission**: Provide feedback on services received.

### For Admins:
- 🔒 **Admin Login**: Secure access to manage the system.
- 🚑 **Ambulance Management**: Add, modify, or remove ambulances.
- 👨‍✈️ **Driver Profiles**: Manage drivers’ information (add/edit/delete).
- 🗺️ **Dispatch Control**: Assign ambulances to requests and monitor live status.
- 👁️ **Real-Time Monitoring**: View all active emergency requests and ambulance locations.
- 📤 **Communication**: Send updates to EMTs and users regarding the status of services.

### For Drivers/EMTs:
- 🔑 **Driver Login**: Secure access to driver and dispatch features.
- 📝 **Patient Information**: Access patients’ medical details and emergency info.
- ⏱️ **Status Updates**: Update ambulance journey status (en route, arrived, transporting).

---

## Specifications

### Non-Functional Requirements:
- **Safety**: The platform guarantees no malicious downloads or unnecessary files.
- **Accessibility**: Clear fonts, legible UI, and simple navigation make it accessible to all.
- **User-Friendliness**: The app offers an intuitive interface, making it easy to use for all age groups.
- **Operability**: The system is reliable, with high uptime and quick response times.
- **Performance**: Fast loading speeds and smooth transitions between pages.
- **Scalability**: Built to handle increasing users and features.
- **Security**: Secure authentication and restricted access to sensitive features.
- **Compatibility**: Works across all modern browsers and devices (mobile, tablet, desktop).

---

## Setup Instructions

To set up and run **RAPID RESCUE** locally, follow the steps below:

### Prerequisites:
- **.NET Core SDK (v3.1 or above)** installed.
- **Visual Studio** with ASP.NET Core MVC support.
- **SQL Server** for the database.

### Installation:

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/syedkashanzikra/RapidRescue.git
   ```

2. **Configure the Database**:
   - Set up an SQL Server database.
   - Update the connection string in `appsettings.json` with your database credentials.

3. **Restore Dependencies**:
   ```bash
   dotnet restore
   ```

4. **Run Database Migrations**:
   ```bash
   dotnet ef database update
   ```

5. **Run the Application**:
   ```bash
   dotnet run
   ```

6. **Access the Application** in your browser:
   ```bash
   http://localhost:7005
   ```

---

## Database Seeding

To quickly set up your local database with roles and users, follow these steps:

1. **Seed Roles**:
   The application includes predefined roles for Admins, Patients, Drivers, and EMTs. To seed these roles into your database, you can run the following method in your `Program.cs` or `Startup.cs` after migrations:

   ```csharp
   using RapidRescue.Data.Seeders;
   using RapidRescue.Context;

   var context = app.ApplicationServices.GetService<RapidRescueContext>();
   RolesSeeder.SeedRoles(context);
   ```

2. **Seed Users**:
   After seeding the roles, seed the users for each role. This includes an admin, a patient, a driver, and an EMT with preset credentials.

   ```csharp
   UsersSeeder.SeedUsers(context);
   ```

3. **Dummy Users and Credentials**:
   You can use the following credentials to log in to the application:

   - **Admin**: 
     - Email: `admin@example.com`
     - Password: `admin123`
   - **Patient**: 
     - Email: `patient@example.com`
     - Password: `patient123`
   - **Driver**: 
     - Email: `driver@example.com`
     - Password: `driver123`
   - **EMT**: 
     - Email: `emt@example.com`
     - Password: `emt123`

---

## Icons & User Interactivity

We’ve integrated **FontAwesome** and **Bootstrap Icons** to enhance user interactivity with clear visual cues:
- 🚨 **Ambulance Requests** with emergency icons.
- 📡 **Real-Time Tracking** with live map icons for easy tracking.
- 📝 **Form Validation** and real-time feedback using **jQuery** to guide users through the input process.

**Example Usage of Icons**:
```html
<button class="btn btn-primary">
    <i class="fas fa-ambulance"></i> Request Ambulance
</button>
```

### Real-Time Interactivity

To make the application more engaging and interactive, we implemented:
- **Real-Time Notifications**: Users and drivers receive instant updates using **SignalR**.
- **Interactive Maps**: Google Maps API is used for real-time ambulance tracking.
- **Live Status Updates**: Drivers can update their status (e.g., "On the way", "Arrived") instantly.

---

## Future Enhancements

- 🚀 **Push Notifications**: Implement push notifications for mobile users.
- 📡 **Optimized Routing**: Use third-party APIs for better ambulance routing and faster response.
- 📊 **Advanced Search Filters**: Allow admins to filter and search requests more efficiently.
- 🎨 **UI Enhancements**: Improve the design for a more modern look and feel.

---

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

## Contact Information

For any inquiries or support, please contact the project team:

- **Syed Kashan Abbas Naqvi (Lead)** 
- **Zayaan Zubair**
- **Prem Kumar**
- **Ahsan Hussain**


We hope **RAPID RESCUE** helps improve emergency response times and saves lives. Stay safe!

---
