import { Navigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const PremiumRoute = ({ children }) => {
    const { user } = useAuth();

    if (!user.isPremium) {
        return <Navigate to="/upgrade" replace />;
    }

    return children;
};

export default PremiumRoute;