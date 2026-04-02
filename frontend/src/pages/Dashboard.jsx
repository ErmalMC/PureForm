import { useState, useCallback, useEffect } from 'react';
import { useAuth } from '../context/AuthContext';
import { workoutApi } from '../api/workoutApi';
import { useNavigate } from 'react-router-dom';
import Navbar from "../components/Navbar.jsx";
import api from '../api/axiosConfig';
import { motion } from 'framer-motion';
import toast from 'react-hot-toast';
import StatCard from '../components/StatCard';
import AnimatedButton from '../components/AnimatedButton';
import LoadingSpinner from '../components/LoadingSpinner';
import { 
    ScaleIcon, 
    HeartIcon, 
    CheckCircleIcon, 
    ClipboardDocumentListIcon,
    SparklesIcon,
    ArrowTrendingUpIcon 
} from '@heroicons/react/24/outline';

const Dashboard = () => {
    const { user } = useAuth();
    const [workoutPlans, setWorkoutPlans] = useState([]);
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();

    const fetchWorkoutPlans = useCallback(async () => {
        try {
            const plans = await workoutApi.getByUserId(user.id);
            setWorkoutPlans(plans);
        } catch (error) {
            console.error('Error fetching workout plans:', error);
        } finally {
            setLoading(false);
        }
    }, [user.id]);

    useEffect(() => {
        fetchWorkoutPlans();
    }, [fetchWorkoutPlans]);

    const handleGeneratePlan = async () => {
        try {
            setLoading(true);
            toast.loading('Generating your personalized plan...');
            await workoutApi.generatePersonalized(user.id, 'Intermediate');
            await fetchWorkoutPlans();
            toast.dismiss();
            toast.success('Plan generated successfully!');
        } catch (error) {
            console.error('Error generating plan:', error);
            console.error('Response data:', error.response?.data);
            console.error('Status:', error.response?.status);
            toast.dismiss();
            toast.error(error.response?.data?.message || 'Failed to generate personalized plan');
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

    return (
        <div className="min-h-screen bg-linear-to-br from-blue-50 via-white to-purple-50">
            {/* Navbar */}
            <Navbar/>

            {/* Main Content */}
            <div className="max-w-7xl mx-auto px-4 py-8">
                {/* Welcome Section */}
                <motion.div
                    className="mb-8"
                    initial={{ opacity: 0, y: -20 }}
                    animate={{ opacity: 1, y: 0 }}
                    transition={{ duration: 0.5 }}
                >
                    <motion.h2
                        className="text-4xl font-bold text-gray-900 mb-2"
                        initial={{ opacity: 0 }}
                        animate={{ opacity: 1 }}
                        transition={{ delay: 0.1 }}
                    >
                        Welcome back, {user.firstName}! 👋
                    </motion.h2>
                    <motion.p
                        className="text-gray-600 text-lg"
                        initial={{ opacity: 0 }}
                        animate={{ opacity: 1 }}
                        transition={{ delay: 0.2 }}
                    >
                        Ready to crush your fitness goals today?
                    </motion.p>
                </motion.div>

                {/* Stats Cards */}
                <motion.div
                    className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 auto-rows-fr gap-6 mb-8"
                    variants={containerVariants}
                    initial="hidden"
                    animate="visible"
                >
                    <StatCard
                        title="Current Weight"
                        value={`${user.weight} kg`}
                        icon={ScaleIcon}
                        color="blue"
                        delay={0}
                    />
                    <StatCard
                        title="Height"
                        value={`${user.height} cm`}
                        icon={HeartIcon}
                        color="purple"
                        delay={0.1}
                    />
                    <StatCard
                        title="Fitness Goal"
                        value={user.fitnessGoal}
                        icon={CheckCircleIcon}
                        color="green"
                        delay={0.2}
                    />
                    <StatCard
                        title="Workout Plans"
                        value={workoutPlans.length}
                        icon={ClipboardDocumentListIcon}
                        color="yellow"
                        delay={0.3}
                    />
                </motion.div>

                {/* Premium Banner */}
                {!user.isPremium && (
                    <motion.div
                        className="rounded-2xl shadow-lg p-[1px] mb-8 bg-linear-to-r from-amber-200 via-orange-200 to-pink-200"
                        initial={{ opacity: 0, y: 20 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ duration: 0.5, delay: 0.4 }}
                        whileHover={{ y: -1 }}
                    >
                        <div className="bg-white rounded-2xl p-6 md:p-8">
                            <div className="grid gap-8 lg:grid-cols-[1.6fr_1fr] lg:items-center">
                                <div className="space-y-5">
                                    <motion.div
                                        className="flex items-center gap-3"
                                        initial={{ opacity: 0, x: -20 }}
                                        animate={{ opacity: 1, x: 0 }}
                                        transition={{ delay: 0.5 }}
                                    >
                                        <span className="inline-flex items-center gap-2 px-3 py-1 rounded-full bg-amber-100 text-amber-700 text-sm font-semibold">
                                            <SparklesIcon className="w-4 h-4" />
                                            Premium Plan
                                        </span>
                                        <span className="text-sm font-semibold text-slate-500">$9.99/month</span>
                                    </motion.div>

                                <motion.h3
                                    className="text-2xl md:text-3xl font-bold text-slate-900 leading-tight"
                                    initial={{ opacity: 0 }}
                                    animate={{ opacity: 1 }}
                                    transition={{ delay: 0.55 }}
                                >
                                    Unlock Premium Features for Faster Results
                                </motion.h3>

                                <motion.p
                                    className="text-slate-600 max-w-2xl"
                                    initial={{ opacity: 0 }}
                                    animate={{ opacity: 1 }}
                                    transition={{ delay: 0.6 }}
                                >
                                    Personalized nutrition and smarter progress tracking, all in one place. Stay
                                    consistent with data-backed guidance built around your goal.
                                </motion.p>

                                <motion.div
                                    className="grid sm:grid-cols-2 gap-3"
                                    initial={{ opacity: 0 }}
                                    animate={{ opacity: 1 }}
                                    transition={{ delay: 0.7 }}
                                >
                                    {['Custom Meal Plans', 'Advanced Analytics', 'Priority Support'].map((feature, index) => (
                                        <motion.div
                                            key={feature}
                                            className="flex items-center gap-2 rounded-xl bg-slate-50 border border-slate-200 px-3 py-2 text-sm font-medium text-slate-700"
                                            initial={{ opacity: 0, y: 8 }}
                                            animate={{ opacity: 1, y: 0 }}
                                            transition={{ delay: 0.7 + index * 0.08 }}
                                        >
                                            <CheckCircleIcon className="w-4 h-4 text-emerald-600" />
                                            <span>{feature}</span>
                                        </motion.div>
                                    ))}
                                </motion.div>
                                </div>

                                <motion.div
                                    className="rounded-2xl border border-slate-200 bg-slate-50 p-5 md:p-6"
                                    initial={{ opacity: 0, x: 16 }}
                                    animate={{ opacity: 1, x: 0 }}
                                    transition={{ delay: 0.75 }}
                                >
                                    <p className="text-sm font-semibold text-slate-500 mb-1">Start Premium</p>
                                    <p className="text-3xl font-bold text-slate-900 mb-4">$9.99<span className="text-base font-medium text-slate-500"> / month</span></p>
                                    <AnimatedButton
                                        variant="primary"
                                        onClick={async () => {
                                            try {
                                                const response = await api.post('/stripe/create-checkout-session', {
                                                    userId: user.id
                                                });

                                                const { url } = response.data;
                                                window.location.href = url;
                                            } catch (error) {
                                                console.error('Error:', error);
                                                    const serverMessage = error?.response?.data?.error || error?.response?.data?.message;
                                                    toast.error(serverMessage || 'Failed to start checkout. Please try again.');
                                            }
                                        }}
                                        className="w-full justify-center"
                                        icon={SparklesIcon}
                                    >
                                        Upgrade Now
                                    </AnimatedButton>
                                    <p className="text-xs text-slate-500 mt-3 text-center">Cancel anytime</p>
                                </motion.div>
                            </div>
                        </div>
                    </motion.div>
                )}

                {/* Quick Actions */}
                <motion.div
                    className="bg-white rounded-2xl shadow-xl p-8 mb-8"
                    initial={{ opacity: 0, y: 20 }}
                    animate={{ opacity: 1, y: 0 }}
                    transition={{ duration: 0.5, delay: 0.5 }}
                >
                    <motion.h3
                        className="text-2xl font-bold text-gray-900 mb-6"
                        initial={{ opacity: 0 }}
                        animate={{ opacity: 1 }}
                        transition={{ delay: 0.6 }}
                    >
                        Quick Actions
                    </motion.h3>
                    <motion.div
                        className="grid grid-cols-1 md:grid-cols-3 gap-4"
                        variants={containerVariants}
                        initial="hidden"
                        animate="visible"
                    >
                        <AnimatedButton
                            variant="primary"
                            onClick={handleGeneratePlan}
                            disabled={loading}
                            icon={SparklesIcon}
                            className="w-full justify-center h-20"
                        >
                            Generate Plan
                        </AnimatedButton>

                        <AnimatedButton
                            variant="success"
                            onClick={() => navigate('/workouts')}
                            className="w-full justify-center h-20"
                            icon={ClipboardDocumentListIcon}
                        >
                            View Workouts
                        </AnimatedButton>

                        <AnimatedButton
                            variant="secondary"
                            onClick={() => navigate('/profile')}
                            className="w-full justify-center h-20"
                            icon={ArrowTrendingUpIcon}
                        >
                            Update Profile
                        </AnimatedButton>
                    </motion.div>
                </motion.div>

                {/* Recent Plans */}
                {loading ? (
                    <motion.div
                        className="flex justify-center py-12"
                        initial={{ opacity: 0 }}
                        animate={{ opacity: 1 }}
                    >
                        <LoadingSpinner size="lg" text="Loading your plans..." />
                    </motion.div>
                ) : workoutPlans.length === 0 ? (
                    <motion.div
                        className="bg-white rounded-2xl shadow-xl p-12 text-center"
                        initial={{ opacity: 0, y: 20 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ delay: 0.6 }}
                    >
                        <motion.div
                            className="inline-flex items-center justify-center w-24 h-24 bg-linear-to-br from-blue-100 to-purple-100 rounded-full mb-6"
                            animate={{ y: [0, -4, 0] }}
                            transition={{ duration: 4, repeat: Infinity, ease: 'easeInOut' }}
                        >
                            <ClipboardDocumentListIcon className="w-12 h-12 text-blue-600" />
                        </motion.div>
                        <motion.h3
                            className="text-2xl font-bold text-gray-900 mb-3"
                            initial={{ opacity: 0 }}
                            animate={{ opacity: 1 }}
                            transition={{ delay: 0.7 }}
                        >
                            No workout plans yet
                        </motion.h3>
                        <motion.p
                            className="text-gray-500 mb-8 max-w-md mx-auto"
                            initial={{ opacity: 0 }}
                            animate={{ opacity: 1 }}
                            transition={{ delay: 0.8 }}
                        >
                            Start your fitness journey by creating a personalized workout plan or let us generate one for you!
                        </motion.p>
                    </motion.div>
                ) : (
                    <motion.div
                        className="bg-white rounded-2xl shadow-xl p-8"
                        initial={{ opacity: 0, y: 20 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ delay: 0.6 }}
                    >
                        <motion.h3
                            className="text-2xl font-bold text-gray-900 mb-6"
                            initial={{ opacity: 0 }}
                            animate={{ opacity: 1 }}
                            transition={{ delay: 0.7 }}
                        >
                            Your Recent Plans
                        </motion.h3>
                        <motion.div
                            className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4"
                            variants={containerVariants}
                            initial="hidden"
                            animate="visible"
                        >
                            {workoutPlans.slice(0, 3).map((plan, index) => (
                                <motion.div
                                    key={plan.id}
                                    className="bg-linear-to-br from-blue-50 to-purple-50 rounded-xl p-4 border-2 border-gray-100 hover:border-blue-500 transition-all cursor-pointer"
                                    onClick={() => navigate('/workouts')}
                                    whileHover={{ scale: 1.02, y: -4 }}
                                    initial={{ opacity: 0, y: 10 }}
                                    animate={{ opacity: 1, y: 0 }}
                                    transition={{ delay: 0.7 + index * 0.1 }}
                                >
                                    <h4 className="font-bold text-gray-900 mb-2">{plan.name}</h4>
                                    <p className="text-sm text-gray-600 mb-3">{plan.description}</p>
                                    <div className="flex items-center justify-between text-xs text-gray-500">
                                        <span>{plan.difficultyLevel}</span>
                                        <span>{plan.durationWeeks} weeks</span>
                                    </div>
                                </motion.div>
                            ))}
                        </motion.div>
                    </motion.div>
                )}
            </div>
        </div>
    );
};

export default Dashboard;
