import { useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { userApi } from '../api/userApi';
import Navbar from '../components/Navbar';
import { motion } from 'framer-motion';
import toast from 'react-hot-toast';
import AnimatedInput from '../components/AnimatedInput';
import AnimatedButton from '../components/AnimatedButton';
import StatCard from '../components/StatCard';

const Profile = () => {
    const { user } = useAuth();
    const [isEditing, setIsEditing] = useState(false);
    const [loading, setLoading] = useState(false);
    const [formData, setFormData] = useState({
        firstName: user.firstName,
        lastName: user.lastName,
        weight: user.weight,
        height: user.height,
        fitnessGoal: user.fitnessGoal
    });
    const [successMessage, setSuccessMessage] = useState('');

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: name === 'weight' || name === 'height' ? parseFloat(value) : value
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        setSuccessMessage('');

        try {
            await userApi.update(user.id, formData);

            const updatedUser = { ...user, ...formData };
            localStorage.setItem('user', JSON.stringify(updatedUser));

            setSuccessMessage('Profile updated successfully!');
            setIsEditing(false);

            setTimeout(() => {
                window.location.reload();
            }, 1500);
        } catch (error) {
            console.error('Error updating profile:', error);
            toast.error('Failed to update profile');
        } finally {
            setLoading(false);
        }
    };

    const handleCancel = () => {
        setFormData({
            firstName: user.firstName,
            lastName: user.lastName,
            weight: user.weight,
            height: user.height,
            fitnessGoal: user.fitnessGoal
        });
        setIsEditing(false);
    };

    const calculateBMI = () => {
        const heightInMeters = user.height / 100;
        const bmi = user.weight / (heightInMeters * heightInMeters);
        return bmi.toFixed(1);
    };

    const getBMICategory = (bmi) => {
        if (bmi < 18.5) return { text: 'Underweight', color: 'text-blue-600', bg: 'bg-blue-100' };
        if (bmi < 25) return { text: 'Normal', color: 'text-green-600', bg: 'bg-green-100' };
        if (bmi < 30) return { text: 'Overweight', color: 'text-yellow-600', bg: 'bg-yellow-100' };
        return { text: 'Obese', color: 'text-red-600', bg: 'bg-red-100' };
    };

    const bmi = calculateBMI();
    const bmiCategory = getBMICategory(parseFloat(bmi));

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
        <div className="min-h-screen bg-gradient-to-br from-blue-50 via-white to-purple-50">
            {/* Navbar */}
            <Navbar/>

            {/* Main Content */}
            <motion.div
                className="max-w-5xl mx-auto px-4 py-8"
                variants={containerVariants}
                initial="hidden"
                animate="visible"
            >
                <motion.div className="bg-white rounded-2xl shadow-2xl overflow-hidden" variants={itemVariants}>
                    {/* Header with Gradient */}
                    <motion.div
                        className="bg-gradient-to-r from-blue-600 via-purple-600 to-blue-600 px-8 py-16 relative overflow-hidden"
                        initial={{ opacity: 0 }}
                        animate={{ opacity: 1 }}
                        transition={{ delay: 0.2 }}
                    >
                        <motion.div
                            className="absolute inset-0 bg-black/10"
                            animate={{ backgroundPosition: ['0%', '100%'] }}
                            transition={{ duration: 8, repeat: Infinity }}
                        />
                        <div className="relative flex items-center justify-between">
                            <motion.div
                                className="flex items-center gap-6"
                                initial={{ opacity: 0, x: -20 }}
                                animate={{ opacity: 1, x: 0 }}
                                transition={{ delay: 0.3 }}
                            >
                                <motion.div
                                    className="bg-white rounded-full p-1 shadow-2xl flex-shrink-0"
                                    animate={{ scale: [1, 1.05, 1] }}
                                    transition={{ duration: 2, repeat: Infinity }}
                                >
                                    <div className="bg-gradient-to-br from-blue-100 to-purple-100 rounded-full p-6">
                                        <svg className="w-16 h-16 text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
                                        </svg>
                                    </div>
                                </motion.div>
                                <motion.div className="text-white" initial={{ opacity: 0 }} animate={{ opacity: 1 }} transition={{ delay: 0.4 }}>
                                    <h2 className="text-4xl font-bold mb-2">{user.firstName} {user.lastName}</h2>
                                    <p className="text-blue-100 text-lg mb-3">{user.email}</p>
                                    <motion.div
                                        className="flex gap-2"
                                        initial={{ opacity: 0, y: 10 }}
                                        animate={{ opacity: 1, y: 0 }}
                                        transition={{ delay: 0.5 }}
                                    >
                                        {user.isPremium ? (
                                            <motion.span
                                                className="bg-gradient-to-r from-yellow-400 to-orange-500 text-white px-4 py-2 rounded-full text-sm font-bold shadow-lg"
                                                animate={{ scale: [1, 1.05, 1] }}
                                                transition={{ duration: 2, repeat: Infinity }}
                                            >
                                                ⭐ Premium Member
                                            </motion.span>
                                        ) : (
                                            <span className="bg-white/20 backdrop-blur-sm text-white px-4 py-2 rounded-full text-sm font-semibold">
                                                Free Member
                                            </span>
                                        )}
                                    </motion.div>
                                </motion.div>
                            </motion.div>
                            {!isEditing && (
                                <AnimatedButton
                                    variant="primary"
                                    onClick={() => setIsEditing(true)}
                                    className="bg-white text-blue-600 hover:bg-blue-50"
                                >
                                    ✏️ Edit Profile
                                </AnimatedButton>
                            )}
                        </div>
                    </motion.div>

                    {/* Success Message */}
                    {successMessage && (
                        <motion.div
                            className="bg-green-100 border border-green-400 text-green-700 px-4 py-3 m-8 rounded flex items-center gap-2"
                            initial={{ opacity: 0, y: -10 }}
                            animate={{ opacity: 1, y: 0 }}
                        >
                            <svg className="w-5 h-5" fill="currentColor" viewBox="0 0 20 20">
                                <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clipRule="evenodd" />
                            </svg>
                            {successMessage}
                        </motion.div>
                    )}

                    {/* Stats Section */}
                    <motion.div
                        className="grid grid-cols-1 md:grid-cols-4 gap-6 p-8 border-b"
                        variants={containerVariants}
                        initial="hidden"
                        animate="visible"
                    >
                        <StatCard
                            title="Weight"
                            value={`${user.weight} kg`}
                            color="blue"
                            delay={0}
                        />
                        <StatCard
                            title="Height"
                            value={`${user.height} cm`}
                            color="purple"
                            delay={0.1}
                        />
                        <StatCard
                            title="BMI"
                            value={bmi}
                            color={bmiCategory.text === 'Normal' ? 'green' : bmiCategory.text === 'Underweight' ? 'blue' : bmiCategory.text === 'Overweight' ? 'yellow' : 'red'}
                            delay={0.2}
                        />
                        <motion.div
                            className={`${bmiCategory.bg} rounded-lg p-4 text-center`}
                            initial={{ opacity: 0, y: 20 }}
                            animate={{ opacity: 1, y: 0 }}
                            transition={{ delay: 0.3 }}
                        >
                            <p className="text-sm font-semibold text-gray-600 mb-1">BMI Category</p>
                            <p className={`text-2xl font-bold ${bmiCategory.color}`}>{bmiCategory.text}</p>
                        </motion.div>
                    </motion.div>

                    {/* Edit Form or Info */}
                    <motion.div
                        className="p-8"
                        initial={{ opacity: 0 }}
                        animate={{ opacity: 1 }}
                        transition={{ delay: 0.4 }}
                    >
                        {isEditing ? (
                            <form onSubmit={handleSubmit} className="space-y-6">
                                <motion.h3
                                    className="text-2xl font-bold text-gray-900"
                                    initial={{ opacity: 0 }}
                                    animate={{ opacity: 1 }}
                                >
                                    Edit Your Profile
                                </motion.h3>

                                <motion.div
                                    className="grid grid-cols-1 sm:grid-cols-2 gap-6"
                                    variants={containerVariants}
                                    initial="hidden"
                                    animate="visible"
                                >
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

                                <motion.div
                                    className="flex gap-4"
                                    variants={itemVariants}
                                >
                                    <AnimatedButton
                                        type="button"
                                        variant="secondary"
                                        onClick={handleCancel}
                                        className="flex-1 justify-center"
                                    >
                                        Cancel
                                    </AnimatedButton>
                                    <AnimatedButton
                                        type="submit"
                                        variant="primary"
                                        disabled={loading}
                                        className="flex-1 justify-center"
                                    >
                                        {loading ? 'Saving...' : 'Save Changes'}
                                    </AnimatedButton>
                                </motion.div>
                            </form>
                        ) : (
                            <motion.div className="space-y-6" variants={containerVariants} initial="hidden" animate="visible">
                                <motion.h3 className="text-2xl font-bold text-gray-900" variants={itemVariants}>
                                    Profile Information
                                </motion.h3>

                                <motion.div className="grid grid-cols-1 md:grid-cols-2 gap-6" variants={containerVariants} initial="hidden" animate="visible">
                                    <motion.div
                                        className="bg-gray-50 rounded-lg p-6"
                                        variants={itemVariants}
                                        whileHover={{ boxShadow: '0 10px 30px rgba(0,0,0,0.1)' }}
                                    >
                                        <p className="text-sm font-semibold text-gray-600 mb-2">First Name</p>
                                        <p className="text-2xl font-bold text-gray-900">{user.firstName}</p>
                                    </motion.div>

                                    <motion.div
                                        className="bg-gray-50 rounded-lg p-6"
                                        variants={itemVariants}
                                        whileHover={{ boxShadow: '0 10px 30px rgba(0,0,0,0.1)' }}
                                    >
                                        <p className="text-sm font-semibold text-gray-600 mb-2">Last Name</p>
                                        <p className="text-2xl font-bold text-gray-900">{user.lastName}</p>
                                    </motion.div>

                                    <motion.div
                                        className="bg-gray-50 rounded-lg p-6"
                                        variants={itemVariants}
                                        whileHover={{ boxShadow: '0 10px 30px rgba(0,0,0,0.1)' }}
                                    >
                                        <p className="text-sm font-semibold text-gray-600 mb-2">Fitness Goal</p>
                                        <p className="text-2xl font-bold text-gray-900">{user.fitnessGoal}</p>
                                    </motion.div>

                                    <motion.div
                                        className="bg-gray-50 rounded-lg p-6"
                                        variants={itemVariants}
                                        whileHover={{ boxShadow: '0 10px 30px rgba(0,0,0,0.1)' }}
                                    >
                                        <p className="text-sm font-semibold text-gray-600 mb-2">Member Since</p>
                                        <p className="text-2xl font-bold text-gray-900">
                                            {new Date(user.createdAt || Date.now()).toLocaleDateString()}
                                        </p>
                                    </motion.div>
                                </motion.div>
                            </motion.div>
                        )}
                    </motion.div>
                </motion.div>
            </motion.div>
        </div>
    );
};

export default Profile;

