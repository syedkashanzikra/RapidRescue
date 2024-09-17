 # RAPID RESCUE - E-Ambulance Web Application

## Project Overview

**RAPID RESCUE** is a responsive and user-friendly web application designed to streamline the process of requesting and dispatching ambulances in emergency and non-emergency situations. The platform allows patients to register, request ambulance services, and track the ambulance's live location on a map. It is intended for individuals, emergency dispatchers, and EMT drivers to manage emergency services efficiently and with real-time updates.

### Key Features

- **User Registration & Profile Management**: Users can create accounts, manage their profile details, and update their medical history.
- **Emergency Request**: Users can request an ambulance by providing details such as hospital name, pickup address, and more.
- **Real-Time Tracking**: Users can track their ambulance's live location and estimated time of arrival.
- **Driver & Dispatch Management**: Admins can manage ambulance dispatch, assign drivers, and track requests in real-time.
- **Patient Information for EMTs**: Drivers and EMTs can access the patient's medical details and update their journey status.
- **First-Aid Instructions**: The app provides basic first-aid instructions to patients while they wait for the ambulance.
- **Feedback & Communication**: Users can submit feedback, and the admin can send updates to EMTs or users during the service.

### Technologies Used

- **ASP.NET Core MVC (C#)**
- **HTML, CSS, Bootstrap**
- **JavaScript, jQuery**
- **Google Maps API (for live tracking)**

---

## Team Members

- **Syed Kashan Abbas Naqvi**
- **Zayaan Zubair**
- **Prem Kumar**
- **Ahsan Hussain**
- **Muhammad Faiq**

---

## Scope of the Project

The **RAPID RESCUE** platform will provide the following functionalities:

### For Users:
1. **Home Page**: Quick access to emergency services and basic information about the app.
2. **Account Registration**: Users can sign up with their email and password.
3. **Profile Management**: Modify profile details and update passwords.
4. **Emergency Request**: Request an ambulance by providing required information (hospital, address, contact details).
5. **Real-Time Tracking**: Track the dispatched ambulance's live location and view directions from pickup to destination.
6. **Medical Profile**: Users can input and update their medical history (allergies, medical conditions, etc.).
7. **First-Aid Instructions**: View basic first-aid guidance for emergencies.
8. **Feedback**: Users can submit feedback on the service received.

### For Admins:
1. **Login**: Secure access to the admin panel.
2. **Ambulance Management**: Add, modify, or remove ambulances.
3. **Driver Profiles**: Manage driver details (add, edit, delete).
4. **Dispatch Control**: Assign ambulances to requests and monitor real-time tracking.
5. **Real-Time Monitoring**: View all active ambulance requests and their current status.
6. **Communication**: Send updates and notifications to EMTs and users.

### For Drivers/EMTs:
1. **Login**: Secure access to their profile and assigned ambulance tasks.
2. **Patient Information**: Access patient medical profiles and details of the emergency.
3. **Status Updates**: Update ambulance status (en-route, arrived, or transporting).

### Common Features:
- **GPS Functionality**: Integration for real-time location tracking and route navigation.
- **Search & Filter**: Search for ambulances, emergency requests, or patients.
- **Notifications**: Alerts for new requests, status changes, and updates.
- **Contact Us**: A form where users can submit queries (name, email, contact number, message).
- **Ambulance Information**: Details on ambulance types, equipment, and services.
- **Image Gallery**: Showcase of ambulances.
- **Driver’s List**: Display a hardcoded list of drivers with contact information.

---

## Specifications

### Non-Functional Requirements:
1. **Safety**: The platform ensures no malicious downloads or unnecessary file transfers.
2. **Accessibility**: Designed with clear fonts, user-interface elements, and easy navigation for all users.
3. **User-Friendliness**: The app offers a seamless experience with intuitive menus and navigation.
4. **Operability**: The platform is reliable, efficient, and available 24/7 with minimal downtime.
5. **Performance**: Ensures high performance with minimal load time and fast page redirection.
6. **Scalability**: The system can handle an increasing number of users and data.
7. **Security**: Includes secure user authentication to restrict access to sensitive features.
8. **Compatibility**: Works across major browsers and devices, ensuring a smooth experience on all platforms.

---

## Setup Instructions

To run this project locally, follow these steps:

### Prerequisites:
- **.NET SDK**: Install .NET Core SDK (v3.1 or above).
- **Visual Studio**: Ensure you have Visual Studio with ASP.NET Core MVC support.
- **SQL Server**: A database for managing users, requests, and ambulance data.

### Steps to Set Up:
1. **Clone the Repository**: Download or clone the project repository from GitHub.
   ```bash
   git clone https://github.com/your-repository/rapid-rescue.git
   ```
2. **Database Setup**:
   - Set up your database in SQL Server.
   - Update the connection string in `appsettings.json` to match your database credentials.
   
3. **Run the Application**:
   - Open the project in Visual Studio.
   - Restore the required packages:
     ```bash
     dotnet restore
     ```
   - Apply any pending migrations to the database:
     ```bash
     dotnet ef database update
     ```
   - Run the application:
     ```bash
     dotnet run
     ```

4. **Access the Application**:
   Once the application is running, open your browser and go to:
   ```bash
   http://localhost:5000
   ```

---

## Features Still in Development

- Enhanced **real-time notifications** for users and admins.
- Integration with **third-party APIs** for route optimization.
- **Advanced search filters** for dispatchers to manage ambulance requests efficiently.

---

## Contribution Guidelines

1. **Fork the repository** and create your branch:
   ```bash
   git checkout -b feature/your-feature
   ```
2. **Commit your changes**:
   ```bash
   git commit -m "Add your feature description"
   ```
3. **Push to the branch**:
   ```bash
   git push origin feature/your-feature
   ```
4. **Open a pull request** to merge your changes.

---

## Contact Information

For any questions or support, please reach out to the project team:

- **Syed Kashan Abbas Naqvi** (Lead Developer)
- **Zayaan Zubair**
- **Prem Kumar**
- **Ahsan Hussain**
- **Muhammad Faiq**

We hope **RAPID RESCUE** helps improve emergency response efficiency and saves lives!
