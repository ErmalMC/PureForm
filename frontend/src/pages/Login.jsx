import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { motion } from 'framer-motion';
import toast from 'react-hot-toast';
import AnimatedButton from '../components/AnimatedButton';
import AnimatedInput from '../components/AnimatedInput';
import LoadingSpinner from '../components/LoadingSpinner';

const Login = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);

    const { login } = useAuth();
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setLoading(true);

        const result = await login(email, password);

        if (result.success) {
            toast.success('Login successful! Welcome back!');
            navigate('/dashboard');
        } else {
            setError(result.error);
            toast.error(result.error);
        }

        setLoading(false);
    };

    const containerVariants = {
        hidden: { opacity: 0 },
        visible: {
            opacity: 1,
            transition: {
                staggerChildren: 0.1,
                delayChildren: 0.2,
            },
        },
    };

    const itemVariants = {
        hidden: { opacity: 0, y: 20 },
        visible: {
            opacity: 1,
            y: 0,
            transition: { duration: 0.5, ease: 'easeOut' },
        },
    };

    return (
        <div className="min-h-screen bg-gradient-to-br from-blue-50 via-white to-purple-50 flex items-center justify-center p-4 relative overflow-hidden">
            {/* Animated background elements */}
            <motion.div
                className="absolute top-0 left-0 w-96 h-96 bg-blue-300 rounded-full mix-blend-multiply filter blur-3xl opacity-20"
                animate={{ y: [0, -50, 0] }}
                transition={{ duration: 8, repeat: Infinity }}
            />
            <motion.div
                className="absolute bottom-0 right-0 w-96 h-96 bg-purple-300 rounded-full mix-blend-multiply filter blur-3xl opacity-20"
                animate={{ y: [0, 50, 0] }}
                transition={{ duration: 8, repeat: Infinity, delay: 1 }}
            />

            <motion.div
                className="w-full max-w-md relative z-10"
                variants={containerVariants}
                initial="hidden"
                animate="visible"
            >
                {/* Logo/Brand */}
                <motion.div className="text-center mb-8" variants={itemVariants}>
                    <motion.div
                        className="inline-flex items-center justify-center w-16 h-16 bg-gradient-to-br from-blue-600 to-purple-600 rounded-2xl mb-4 shadow-lg"
                        animate={{ rotate: 360 }}
                        transition={{ duration: 20, repeat: Infinity, ease: 'linear' }}
                    >
                        <svg className="w-10 h-10 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M13 10V3L4 14h7v7l9-11h-7z" />
                        </svg>
                    </motion.div>
                    <motion.h1
                        className="text-4xl font-bold bg-gradient-to-r from-blue-600 to-purple-600 bg-clip-text text-transparent"
                        initial={{ scale: 0.9, opacity: 0 }}
                        animate={{ scale: 1, opacity: 1 }}
                        transition={{ duration: 0.5 }}
                    >
                        PureForm
                    </motion.h1>
                    <motion.p
                        className="text-gray-600 mt-2"
                        initial={{ opacity: 0 }}
                        animate={{ opacity: 1 }}
                        transition={{ delay: 0.2 }}
                    >
                        Your fitness journey starts here
                    </motion.p>
                </motion.div>

                {/* Login Card */}
                <motion.div
                    className="bg-white/90 rounded-2xl shadow-2xl p-8 backdrop-blur-lg"
                    variants={itemVariants}
                    whileHover={{ boxShadow: '0 25px 50px -12px rgba(0, 0, 0, 0.15)' }}
                >
                    <motion.h2 className="text-2xl font-bold text-gray-900 mb-6" variants={itemVariants}>
                        Welcome Back
                    </motion.h2>

                    {error && (
                        <motion.div
                            className="bg-red-50 border-l-4 border-red-500 text-red-700 p-4 rounded-lg mb-6"
                            initial={{ opacity: 0, x: -20 }}
                            animate={{ opacity: 1, x: 0 }}
                            exit={{ opacity: 0, x: -20 }}
                        >
                            <div className="flex items-center">
                                <svg className="w-5 h-5 mr-2" fill="currentColor" viewBox="0 0 20 20">
                                    <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clipRule="evenodd" />
                                </svg>
                                {error}
                            </div>
                        </motion.div>
                    )}

                    <motion.form onSubmit={handleSubmit} className="space-y-5" variants={itemVariants}>
                        <AnimatedInput
                            label="Email Address"
                            type="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            placeholder="you@example.com"
                            required
                        />

                        <AnimatedInput
                            label="Password"
                            type="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            placeholder="••••••••"
                            required
                        />

                        <AnimatedButton
                            type="submit"
                            disabled={loading}
                            variant="primary"
                            className="w-full justify-center"
                        >
                            {loading ? (
                                <motion.div animate={{ rotate: 360 }} transition={{ duration: 1, repeat: Infinity }}>
                                    <svg className="h-5 w-5" fill="currentColor" viewBox="0 0 20 20">
                                        <path fillRule="evenodd" d="M11.3 1.046A1 1 0 0010 2v3.5H5.5a1 1 0 00-.707 1.707l3.5 3.5a1 1 0 101.414-1.414L7.914 7H10v3.5a1 1 0 001 1h3.5l-2.293 2.293a1 1 0 101.414 1.414l3.5-3.5a1 1 0 00.281-1.094A1 1 0 0016 12v-3.5h3.5a1 1 0 00.707-1.707l-3.5-3.5a1 1 0 101.414 1.414L18.086 7H20V3.5a1 1 0 00-1.707-.707l-3.5 3.5a1 1 0 101.414 1.414L18.086 7H14.5V3.5a1 1 0 00-.354-.854z" clipRule="evenodd" />
                                    </svg>
                                </motion.div>
                            ) : (
                                'Sign In'
                            )}
                        </AnimatedButton>
                    </motion.form>

                    <motion.div className="mt-6 text-center" variants={itemVariants}>
                        <p className="text-gray-600">
                            Don't have an account?{' '}
                            <Link to="/register" className="font-semibold text-blue-600 hover:text-blue-700 transition-colors">
                                Create one now
                            </Link>
                        </p>
                    </motion.div>
                </motion.div>

                {/* Footer */}
                <motion.p className="text-center text-gray-500 text-sm mt-8" variants={itemVariants}>
                    © 2026 PureForm. All rights reserved.
                </motion.p>
            </motion.div>
        </div>
    );
};

export default Login;