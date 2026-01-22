# PureForm 🏋️‍♂️

A modern full-stack fitness and nutrition tracking platform built with .NET 8 and React. Features intelligent workout logging, comprehensive nutrition tracking with AI-powered macro recommendations, and data-driven progress analytics.

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![React](https://img.shields.io/badge/React-18.3-61DAFB?logo=react)](https://reactjs.org/)
[![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1?logo=mysql)](https://www.mysql.com/)

## 🎯 Key Features

- **Workout Management**: Custom routines, exercise logging with sets/reps/weight, progress tracking with visual charts
- **Nutrition Tracking**: 100+ food database, smart search, meal logging by type, AI-powered macro calculator
- **Progress Analytics**: Body metrics tracking, trend analysis, goal achievement system with visual dashboards
- **Modern UX**: Responsive design, real-time updates, intuitive interface with micro-interactions

## 🛠️ Tech Stack

**Frontend**: React 18 • TailwindCSS • React Router • Axios • Recharts  
**Backend**: .NET 8 • ASP.NET Core Web API • Entity Framework Core • JWT Auth • BCrypt  
**Database**: MySQL 8.0  
**Architecture**: Clean Architecture with Repository Pattern, DTOs, Dependency Injection

## 🏗️ Architecture Highlights

- **Clean Separation**: Domain, Application, Infrastructure, and Presentation layers
- **Repository Pattern**: Abstraction over data access for testability
- **RESTful API**: Proper HTTP methods, status codes, and endpoint design
- **Security**: JWT authentication, BCrypt password hashing, CORS configuration
- **Async Operations**: Non-blocking I/O for better scalability

## 🚀 Quick Start

### Prerequisites
Node.js 18+, .NET 8 SDK, MySQL 8.0+

### Backend
```bash
cd backend/PureForm.WebAPI
dotnet restore
# Update connection string in appsettings.json
dotnet ef database update --project ../PureForm.Infrastructure
dotnet run  # https://localhost:5152
```

### Frontend
```bash
cd frontend
npm install
echo "VITE_API_BASE_URL=https://localhost:5152" > .env
npm run dev  # http://localhost:5173
```

## 📡 Core API Endpoints

**Auth**: `POST /api/auth/register`, `POST /api/auth/login`  
**Users**: `GET /api/users/{id}`, `PUT /api/users/{id}`  
**Workouts**: `GET/POST /api/workouts/user/{userId}`, `PUT/DELETE /api/workouts/{id}`  
**Nutrition**: `GET/POST /api/nutrition/user/{userId}`, `GET /api/nutrition/user/{userId}/daily-totals`  
**Food DB**: `GET /api/fooditems/popular`, `GET /api/fooditems/search?query={q}`  
**Calculator**: `GET /api/nutritioncalculator/recommendations/{userId}`

## 📈 Database Schema

**Users** - Profile, fitness goals, daily macro targets  
**Workouts** - Name, date, duration, notes  
**Exercises** - Exercise library with categories and muscle groups  
**WorkoutExercises** - Sets, reps, weight per exercise  
**NutritionLogs** - Meals with complete macro breakdown  
**FoodItems** - Comprehensive food database with nutritional info

## 💼 What This Demonstrates

✅ Full-stack development from database to UI  
✅ Clean architecture and SOLID principles  
✅ Modern .NET practices (DI, async/await, EF Core)  
✅ React hooks and context API expertise  
✅ RESTful API design and implementation  
✅ Database modeling and migrations  
✅ Authentication and authorization  
✅ Responsive, production-ready UI


## 👤 Contact

**Ermal Baki** - Full-Stack Developer

💼 [LinkedIn](https://www.linkedin.com/in/ermal-baki/) • 🐙 [GitHub](https://github.com/ErmalMC) • 🌐 [Portfolio](https://ermalmc.github.io/)

---

⭐ **Star this repo** if you find it helpful!  
