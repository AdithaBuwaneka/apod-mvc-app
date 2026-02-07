# APOD Gallery - NASA Astronomy Picture of the Day

An ASP.NET MVC application that consumes NASA's APOD (Astronomy Picture of the Day) API, stores the data in SQL Server, and displays images in a responsive gallery.

## 🚀 Features

- **Fetch Data:**
  - One-click "Fetch Today's APOD"
  - Fetch multiple APODs by custom date range
- **Gallery Display:**
  - Modern responsive CSS Grid layout
  - ✨ **Pagination** for easy browsing (12 items per page)
  - 📅 **Date Filtering** to view specific time periods
- **Detailed View:**
  - 🔍 **Interactive Popup Modal** showing full details (Title, Date, Explanation, Copyright)
  - HD Image viewing option
- **Backend:**
  - SQL Server storage using raw ADO.NET (required for assignment)
  - Duplicate entry prevention logic
- **UX Improvements:**
  - Loading skeletons and placeholders
  - Dismissible alert messages
  - "Show All" button to clear filters

## 📋 Prerequisites

Before running this project, ensure you have:

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download) installed
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or SQL Server Express installed and running
- [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) (optional, for database management)

## 🛠️ Setup Instructions

### 1. Clone the Repository

```bash
git clone https://github.com/AdithaBuwaneka/apod-mvc-app.git
cd apod-mvc-app
```

### 2. Create the Database

Open SQL Server Management Studio (SSMS) or use PowerShell:

**Option A: Using SSMS**
1. Connect to your SQL Server instance
2. Open `Scripts/CreateApodTable.sql`
3. First, create the database:
   ```sql
   CREATE DATABASE ApodDb;
   GO
   ```
4. Then execute the script to create the table

**Option B: Using PowerShell**
```powershell
# Create database
Invoke-Sqlcmd -Query "CREATE DATABASE ApodDb" -ServerInstance "localhost"

# Create table (run the SQL script)
Invoke-Sqlcmd -InputFile "Scripts/CreateApodTable.sql" -ServerInstance "localhost"
```

### 3. Configure NASA API Key

1. Go to [NASA API Portal](https://api.nasa.gov/)
2. Generate your free API key
3. Create `appsettings.Development.json` in the project root:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "NasaApi": {
    "ApiKey": "YOUR_NASA_API_KEY_HERE"
  }
}
```

> ⚠️ **Note:** Never commit your API key to source control. The `appsettings.Development.json` file is already in `.gitignore`.

### 4. Configure Database Connection (if needed)

The default connection string in `appsettings.json` uses Windows Authentication:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=ApodDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Modify if your SQL Server uses different settings.

### 5. Run the Application

```bash
cd apod-mvc-app
dotnet run
```
or use `dotnet watch run` for hot reloading during development.

The application will start at: **http://localhost:5200**

## 📁 Project Structure

```
apod-mvc-app/
├── Controllers/
│   └── HomeController.cs       # Main controller with Index, FetchToday, FetchRange
├── DTOs/
│   └── ApodDto.cs              # Data Transfer Object for API response
├── Models/
│   └── ErrorViewModel.cs       # Error handling model
├── Repositories/
│   ├── IApodRepository.cs      # Repository interface
│   └── ApodRepository.cs       # ADO.NET implementation (SqlConnection, SqlCommand)
├── Services/
│   ├── IApodService.cs         # Service interface
│   └── ApodService.cs          # NASA API client
├── Views/
│   ├── Home/
│   │   └── Index.cshtml        # Gallery view with Modal & Pagination
│   └── Shared/
│       └── _Layout.cshtml      # Layout template
├── Scripts/
│   └── CreateApodTable.sql     # Database creation script
├── wwwroot/
│   └── css/
│       └── site.css            # Responsive styles & Modal CSS
├── appsettings.json            # Configuration (no secrets)
├── Program.cs                  # Application entry point & DI container
└── README.md                   # This file
```

## 🗄️ Database Schema

```sql
CREATE TABLE Apod (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Date DATE NOT NULL UNIQUE,
    Title NVARCHAR(500) NOT NULL,
    Explanation NVARCHAR(MAX) NOT NULL,
    Url NVARCHAR(1000) NOT NULL,
    HdUrl NVARCHAR(1000) NULL,
    MediaType NVARCHAR(50) NOT NULL,
    ServiceVersion NVARCHAR(20) NOT NULL,
    Copyright NVARCHAR(200) NULL,
    ThumbnailUrl NVARCHAR(1000) NULL,
    SavedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);
```

## 🔧 Technologies Used

- **Framework:** ASP.NET Core MVC (.NET 8+)
- **Database:** SQL Server with ADO.NET (SqlConnection, SqlCommand, SqlParameter)
- **Frontend:** HTML, CSS (no frameworks), responsive CSS Grid
- **API:** NASA APOD API

## 📝 Usage

1. **Browse Gallery:** Use pagination controls at the bottom to navigate through images.
2. **View Details:** Click on any image to open a popup with full description and HD link.
3. **Fetch New Data:**
   - Click **"Fetch Today's APOD"** for the latest image
   - Use the date range picker to fetch a batch of past images
4. **Filter:** After fetching storage, click "Show All" to reset filters.

## 📄 License

This project is for educational purposes as part of a technical assignment.
