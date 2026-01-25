import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import ProtectedRoute from './components/ProtectedRoute';
import PremiumRoute from './components/PremiumRoute';
import Login from './pages/Login';
import Register from './pages/Register';
import Dashboard from './pages/Dashboard';
import WorkoutPlans from './pages/WorkoutPlans';
import Profile from './pages/Profile';
import Nutrition from "./pages/Nutrition.jsx";
import PaymentSuccess from "./pages/PaymentSuccess.jsx";
import PaymentCancel from "./pages/PaymentCancel.jsx";
import Upgrade from "./pages/Upgrade.jsx";

function App() {
    return (
        <BrowserRouter>
            <AuthProvider>
                <Routes>
                    <Route path="/success" element={<PaymentSuccess />} />
                    <Route path="/cancel" element={<PaymentCancel />} />
                    <Route path="/login" element={<Login />} />
                    <Route path="/register" element={<Register />} />

                    <Route path="/dashboard" element={<ProtectedRoute><Dashboard /></ProtectedRoute>}/>
                    <Route path="/upgrade" element={<ProtectedRoute><Upgrade /></ProtectedRoute>}/>

                    <Route path="/nutrition" element={<ProtectedRoute><PremiumRoute><Nutrition /></PremiumRoute></ProtectedRoute>}/>

                    <Route path="/workouts" element={<ProtectedRoute><WorkoutPlans /></ProtectedRoute>}/>

                    <Route path="/profile" element={<ProtectedRoute><Profile /></ProtectedRoute>}/>

                    <Route path="/" element={<Navigate to="/dashboard" replace />} />
                </Routes>
            </AuthProvider>
        </BrowserRouter>
    );
}

export default App;