import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { motion, AnimatePresence } from 'framer-motion';
import toast from 'react-hot-toast';
import AnimatedInput from '../components/AnimatedInput';
import AnimatedButton from '../components/AnimatedButton';

const Register = () => {
    const [step, setStep] = useState(1);
    const [formData, setFormData] = useState({
        email: '',
        password: '',
        firstName: '',
        lastName: '',
        dateOfBirth: '',
        weight: '',
        height: '',
        gender: 'Male',
        fitnessGoal: 'MuscleGain'
    });
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);

    const { register } = useAuth();
    const navigate = useNavigate();

    const handleChange = (e) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value
        });
    };

    const handleNext = () => {
        if (step === 1 && (!formData.email || !formData.password || !formData.firstName || !formData.lastName)) {
            setError('Please fill in all fields');
            toast.error('Please fill in all fields');
            return;
        }
        setError('');
        setStep(2);
    };

    const handleBack = () => {
        setError('');
        setStep(1);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setLoading(true);
        const toastId = toast.loading('Creating your account...');

        const userData = {
            ...formData,
            weight: parseFloat(formData.weight),
            height: parseFloat(formData.height)
        };

        try {
            const result = await register(userData);

            if (result.success) {
                toast.success('Account created successfully! Welcome to PureForm!', { id: toastId });
                navigate('/dashboard');
                return;
            }

            setError(result.error);
            toast.error(result.error || 'Registration failed. Please try again.', { id: toastId });
        } catch {
            const fallbackMessage = 'Unable to create account right now. Please try again.';
            setError(fallbackMessage);
            toast.error(fallbackMessage, { id: toastId });
        } finally {
            setLoading(false);
        }
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
        <div className="min-h-screen bg-linear-to-br from-slate-100 via-white to-blue-100 flex items-center justify-center p-4 relative overflow-hidden">
            {/* Animated background elements */}
            <motion.div
                className="absolute -top-16 -left-12 w-80 h-80 bg-blue-200 rounded-full blur-3xl opacity-35"
                animate={{ y: [0, -50, 0] }}
                transition={{ duration: 12, repeat: Infinity }}
            />
            <motion.div
                className="absolute -bottom-16 -right-12 w-80 h-80 bg-indigo-200 rounded-full blur-3xl opacity-35"
                animate={{ y: [0, 50, 0] }}
                transition={{ duration: 12, repeat: Infinity, delay: 1.5 }}
            />

            <motion.div
                className="w-full max-w-2xl relative z-10"
                variants={containerVariants}
                initial="hidden"
                animate="visible"
            >
                {/* Logo/Brand */}
                <motion.div className="text-center mb-8" variants={itemVariants}>
                    <motion.div
                        className="inline-flex items-center justify-center w-16 h-16 bg-linear-to-br from-blue-600 to-indigo-600 rounded-2xl mb-4 shadow-md"
                        initial={{ opacity: 0, y: 8 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ duration: 0.45 }}
                    >
                        <svg className="w-10 h-10 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M13 10V3L4 14h7v7l9-11h-7z" />
                        </svg>
                    </motion.div>
                    <motion.h1
                        className="text-4xl font-bold bg-linear-to-r from-slate-800 to-blue-700 bg-clip-text text-transparent"
                        initial={{ scale: 0.9 }}
                        animate={{ scale: 1 }}
                        transition={{ duration: 0.5 }}
                    >
                        Join PureForm
                    </motion.h1>
                    <motion.p
                        className="text-gray-600 mt-2"
                        initial={{ opacity: 0 }}
                        animate={{ opacity: 1 }}
                        transition={{ delay: 0.2 }}
                    >
                        Start your transformation today
                    </motion.p>
                </motion.div>

                {/* Progress Indicator */}
                <motion.div className="mb-8" variants={itemVariants}>
                    <div className="flex items-center justify-center">
                        <motion.div
                            className={`flex items-center ${step >= 1 ? 'text-blue-600' : 'text-gray-400'}`}
                            animate={step >= 1 ? { scale: 1.1 } : { scale: 1 }}
                        >
                            <motion.div
                                className={`w-10 h-10 rounded-full flex items-center justify-center font-bold transition-all ${step >= 1 ? 'bg-blue-600 text-white shadow-lg' : 'bg-gray-300'}`}
                                animate={step >= 1 ? { scale: 1 } : { scale: 1 }}
                            >
                                1
                            </motion.div>
                            <span className="ml-2 font-semibold hidden sm:inline">Account</span>
                        </motion.div>
                        <motion.div
                            className={`w-16 h-1 mx-4 transition-all ${step >= 2 ? 'bg-blue-600' : 'bg-gray-300'}`}
                            animate={step >= 2 ? { width: 100 } : { width: 60 }}
                        />
                        <motion.div
                            className={`flex items-center ${step >= 2 ? 'text-blue-600' : 'text-gray-400'}`}
                            animate={step >= 2 ? { scale: 1.1 } : { scale: 1 }}
                        >
                            <motion.div
                                className={`w-10 h-10 rounded-full flex items-center justify-center font-bold transition-all ${step >= 2 ? 'bg-blue-600 text-white shadow-lg' : 'bg-gray-300'}`}
                            >
                                2
                            </motion.div>
                            <span className="ml-2 font-semibold hidden sm:inline">Profile</span>
                        </motion.div>
                    </div>
                </motion.div>

                {/* Register Card */}
                <motion.div
                    className="bg-white/95 border border-slate-200 rounded-2xl shadow-xl p-8"
                    variants={itemVariants}
                    whileHover={{ y: -2 }}
                >
                    <AnimatePresence mode="wait">
                        {error && (
                            <motion.div
                                role="alert"
                                className="bg-red-50 border border-red-200 text-red-700 p-4 rounded-lg mb-6"
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
                    </AnimatePresence>

                    <form onSubmit={handleSubmit}>
                        <AnimatePresence mode="wait">
                            {step === 1 ? (
                                <motion.div
                                    key="step1"
                                    className="space-y-5"
                                    initial={{ opacity: 0, x: 20 }}
                                    animate={{ opacity: 1, x: 0 }}
                                    exit={{ opacity: 0, x: -20 }}
                                    transition={{ duration: 0.3 }}
                                >
                                    <motion.h2 className="text-2xl font-bold text-gray-900 mb-6" variants={itemVariants}>
                                        Create Your Account
                                    </motion.h2>

                                    <motion.div className="grid grid-cols-1 sm:grid-cols-2 gap-4" variants={containerVariants} initial="hidden" animate="visible">
                                        <AnimatedInput
                                            label="First Name"
                                            type="text"
                                            name="firstName"
                                            value={formData.firstName}
                                            onChange={handleChange}
                                            required
                                        />
                                        <AnimatedInput
                                            label="Last Name"
                                            type="text"
                                            name="lastName"
                                            value={formData.lastName}
                                            onChange={handleChange}
                                            required
                                        />
                                    </motion.div>

                                    <AnimatedInput
                                        label="Email Address"
                                        type="email"
                                        name="email"
                                        value={formData.email}
                                        onChange={handleChange}
                                        placeholder="you@example.com"
                                        required
                                    />

                                    <AnimatedInput
                                        label="Password"
                                        type="password"
                                        name="password"
                                        value={formData.password}
                                        onChange={handleChange}
                                        placeholder="••••••••"
                                        required
                                    />

                                    <AnimatedButton
                                        type="button"
                                        variant="primary"
                                        onClick={handleNext}
                                        className="w-full justify-center"
                                    >
                                        Next Step →
                                    </AnimatedButton>
                                </motion.div>
                            ) : (
                                <motion.div
                                    key="step2"
                                    className="space-y-5"
                                    initial={{ opacity: 0, x: 20 }}
                                    animate={{ opacity: 1, x: 0 }}
                                    exit={{ opacity: 0, x: -20 }}
                                    transition={{ duration: 0.3 }}
                                >
                                    <motion.h2 className="text-2xl font-bold text-gray-900 mb-6" variants={itemVariants}>
                                        Complete Your Profile
                                    </motion.h2>

                                    <AnimatedInput
                                        label="Date of Birth"
                                        type="date"
                                        name="dateOfBirth"
                                        value={formData.dateOfBirth}
                                        onChange={handleChange}
                                        required
                                    />

                                    <motion.div className="grid grid-cols-1 sm:grid-cols-2 gap-4" variants={containerVariants} initial="hidden" animate="visible">
                                        <AnimatedInput
                                            label="Weight (kg)"
                                            type="number"
                                            step="0.1"
                                            name="weight"
                                            value={formData.weight}
                                            onChange={handleChange}
                                            required
                                        />
                                        <AnimatedInput
                                            label="Height (cm)"
                                            type="number"
                                            step="0.1"
                                            name="height"
                                            value={formData.height}
                                            onChange={handleChange}
                                            required
                                        />
                                    </motion.div>

                                    <motion.div variants={itemVariants}>
                                        <label className="block text-sm font-semibold text-gray-700 mb-2">Gender</label>
                                        <select
                                            name="gender"
                                            value={formData.gender}
                                            onChange={handleChange}
                                            className="w-full px-4 py-3 bg-gray-50 border-2 border-gray-300 rounded-xl focus:outline-none focus:ring-0 focus:border-blue-500 transition-all"
                                        >
                                            <option value="Male">Male</option>
                                            <option value="Female">Female</option>
                                            <option value="Other">Other</option>
                                        </select>
                                    </motion.div>

                                    <motion.div variants={itemVariants}>
                                        <label className="block text-sm font-semibold text-gray-700 mb-2">Fitness Goal</label>
                                        <select
                                            name="fitnessGoal"
                                            value={formData.fitnessGoal}
                                            onChange={handleChange}
                                            className="w-full px-4 py-3 bg-gray-50 border-2 border-gray-300 rounded-xl focus:outline-none focus:ring-0 focus:border-blue-500 transition-all"
                                        >
                                            <option value="WeightLoss">🔥 Weight Loss</option>
                                            <option value="MuscleGain">💪 Muscle Gain</option>
                                            <option value="GeneralFitness">⚡ General Fitness</option>
                                            <option value="Endurance">🏃 Endurance</option>
                                        </select>
                                    </motion.div>

                                    <motion.div className="flex gap-4" variants={itemVariants}>
                                        <AnimatedButton
                                            type="button"
                                            variant="secondary"
                                            onClick={handleBack}
                                            className="flex-1 justify-center"
                                        >
                                            ← Back
                                        </AnimatedButton>
                                        <AnimatedButton
                                            type="submit"
                                            variant="primary"
                                            disabled={loading}
                                            className="flex-1 justify-center"
                                        >
                                            {loading ? 'Creating...' : 'Complete Registration'}
                                        </AnimatedButton>
                                    </motion.div>
                                </motion.div>
                            )}
                        </AnimatePresence>
                    </form>

                    <motion.div className="mt-6 text-center" variants={itemVariants}>
                        <p className="text-gray-600">
                            Already have an account?{' '}
                            <Link to="/login" className="font-semibold text-blue-600 hover:text-blue-700 transition-colors">
                                Sign in
                            </Link>
                        </p>
                    </motion.div>
                </motion.div>
            </motion.div>
        </div>
    );
};

export default Register;