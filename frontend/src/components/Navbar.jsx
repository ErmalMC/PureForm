import { useNavigate, useLocation } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { motion } from 'framer-motion';

const Navbar = () => {
    const { user, logout } = useAuth();
    const navigate = useNavigate();
    const location = useLocation();

    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    const isActive = (path) => location.pathname === path;

    const navItems = [
        { path: '/dashboard', label: 'Dashboard' },
        { path: '/workouts', label: 'Workouts' },
        { path: '/profile', label: 'Profile' },
        ...(user?.isPremium ? [{ path: '/nutrition', label: 'Nutrition' }] : []),
    ];

    return (
        <nav className="bg-white/80 backdrop-blur-lg shadow-lg sticky top-0 z-50 border-b border-gray-200">
            <div className="max-w-7xl mx-auto px-4 py-4">
                <div className="flex justify-between items-center">
                    {/* Logo */}
                    <motion.div
                        className="flex items-center gap-3 cursor-pointer"
                        onClick={() => navigate('/dashboard')}
                        whileHover={{ scale: 1.05 }}
                        whileTap={{ scale: 0.95 }}
                    >
                        <motion.div
                            className="w-10 h-10 bg-gradient-to-br from-blue-600 to-purple-600 rounded-xl flex items-center justify-center shadow-lg"
                            animate={{ rotate: [0, 360] }}
                            transition={{ duration: 20, repeat: Infinity, ease: 'linear' }}
                        >
                            <svg className="w-6 h-6 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M13 10V3L4 14h7v7l9-11h-7z" />
                            </svg>
                        </motion.div>
                        <motion.h1
                            className="text-2xl font-bold bg-gradient-to-r from-blue-600 to-purple-600 bg-clip-text text-transparent"
                            initial={{ opacity: 0, x: -20 }}
                            animate={{ opacity: 1, x: 0 }}
                            transition={{ duration: 0.5 }}
                        >
                            PureForm
                        </motion.h1>
                    </motion.div>

                    {/* Navigation Links */}
                    <div className="flex items-center gap-2">
                        {navItems.map((item, index) => (
                            <motion.button
                                key={item.path}
                                onClick={() => navigate(item.path)}
                                className={`px-4 py-2 rounded-lg font-semibold transition-all relative overflow-hidden ${
                                    isActive(item.path)
                                        ? 'text-white'
                                        : 'text-gray-600 hover:text-gray-900'
                                }`}
                                whileHover={{ scale: 1.05 }}
                                whileTap={{ scale: 0.95 }}
                                initial={{ opacity: 0, y: -10 }}
                                animate={{ opacity: 1, y: 0 }}
                                transition={{ delay: index * 0.05 }}
                            >
                                {isActive(item.path) && (
                                    <motion.div
                                        className="absolute inset-0 bg-gradient-to-r from-blue-600 to-purple-600 rounded-lg -z-10"
                                        layoutId="navbar-highlight"
                                        transition={{ duration: 0.3 }}
                                    />
                                )}
                                {item.label}
                            </motion.button>
                        ))}

                        {/* User Info */}
                        <motion.div
                            className="flex items-center gap-3 pl-4 ml-4 border-l-2 border-gray-200"
                            initial={{ opacity: 0, x: 20 }}
                            animate={{ opacity: 1, x: 0 }}
                            transition={{ duration: 0.5, delay: 0.2 }}
                        >
                            <div className="text-right hidden sm:block">
                                <motion.p
                                    className="text-sm font-semibold text-gray-900"
                                    initial={{ opacity: 0 }}
                                    animate={{ opacity: 1 }}
                                    transition={{ delay: 0.3 }}
                                >
                                    {user?.firstName}
                                </motion.p>
                                {user?.isPremium && (
                                    <motion.span
                                        className="inline-block bg-gradient-to-r from-yellow-400 to-orange-500 text-white px-2 py-0.5 rounded-full text-xs font-bold"
                                        initial={{ scale: 0 }}
                                        animate={{ scale: 1 }}
                                        transition={{ type: 'spring', delay: 0.4 }}
                                    >
                                        ⭐ Premium
                                    </motion.span>
                                )}
                            </div>
                            <motion.button
                                onClick={handleLogout}
                                className="bg-gradient-to-r from-red-500 to-red-600 text-white px-4 py-2 rounded-xl font-semibold hover:from-red-600 hover:to-red-700 transition-all shadow-lg hover:shadow-xl"
                                whileHover={{ scale: 1.05 }}
                                whileTap={{ scale: 0.95 }}
                            >
                                Logout
                            </motion.button>
                        </motion.div>
                    </div>
                </div>
            </div>
        </nav>
    );
};

export default Navbar;