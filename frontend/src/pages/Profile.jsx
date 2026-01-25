import { useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { userApi } from '../api/userApi';
import Navbar from '../components/Navbar';

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
            alert('Failed to update profile');
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

    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    const calculateBMI = () => {
        const heightInMeters = user.height / 100;
        const bmi = user.weight / (heightInMeters * heightInMeters);
        return bmi.toFixed(1);
    };

    const getBMICategory = (bmi) => {
        if (bmi < 18.5) return { text: 'Underweight', color: 'text-blue-600' };
        if (bmi < 25) return { text: 'Normal', color: 'text-green-600' };
        if (bmi < 30) return { text: 'Overweight', color: 'text-yellow-600' };
        return { text: 'Obese', color: 'text-red-600' };
    };

    const bmi = calculateBMI();
    const bmiCategory = getBMICategory(parseFloat(bmi));

    return (
        <div className="min-h-screen bg-gray-100">
            {/* Navbar */}
            <Navbar/>

            {/* Main Content */}
            <div className="max-w-5xl mx-auto px-4 py-8">
                <div className="bg-white rounded-2xl shadow-2xl overflow-hidden">
                    {/* Header with Gradient */}
                    <div className="bg-gradient-to-r from-blue-600 via-purple-600 to-blue-600 px-8 py-16 relative overflow-hidden">
                        <div className="absolute inset-0 bg-black/10"></div>
                        <div className="relative flex items-center justify-between">
                            <div className="flex items-center gap-6">
                                <div className="bg-white rounded-full p-1 shadow-2xl flex-shrink-0">
                                    <div className="bg-gradient-to-br from-blue-100 to-purple-100 rounded-full p-6">
                                        <svg className="w-16 h-16 text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
                                        </svg>
                                    </div>
                                </div>
                                <div className="text-white">
                                    <h2 className="text-4xl font-bold mb-2">{user.firstName} {user.lastName}</h2>
                                    <p className="text-blue-100 text-lg mb-3">{user.email}</p>
                                    <div className="flex gap-2">
                                        {user.isPremium ? (
                                            <span className="bg-gradient-to-r from-yellow-400 to-orange-500 text-white px-4 py-2 rounded-full text-sm font-bold shadow-lg">
                        ⭐ Premium Member
                      </span>
                                        ) : (
                                            <span className="bg-white/20 backdrop-blur-sm text-white px-4 py-2 rounded-full text-sm font-semibold">
                        Free Member
                      </span>
                                        )}
                                    </div>
                                </div>
                            </div>
                            {!isEditing && (
                                <button
                                    onClick={() => setIsEditing(true)}
                                    className="bg-white text-blue-600 px-6 py-3 rounded-xl hover:bg-blue-50 font-bold shadow-xl transform hover:scale-105 transition-all flex items-center gap-2"
                                >
                                    <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
                                    </svg>
                                    Edit Profile
                                </button>
                            )}
                        </div>
                    </div>

                    {/* Success Message */}
                    {successMessage && (
                        <div className="bg-green-100 border border-green-400 text-green-700 px-4 py-3 m-8 rounded">
                            {successMessage}
                        </div>
                    )}

                    {/* Stats Section */}
                    <div className="grid grid-cols-1 md:grid-cols-3 gap-6 p-8 border-b">
                        <div className="text-center">
                            <div className="text-3xl font-bold text-blue-600">{user.weight} kg</div>
                            <div className="text-gray-600 mt-1">Current Weight</div>
                        </div>
                        <div className="text-center">
                            <div className="text-3xl font-bold text-blue-600">{user.height} cm</div>
                            <div className="text-gray-600 mt-1">Height</div>
                        </div>
                        <div className="text-center">
                            <div className={`text-3xl font-bold ${bmiCategory.color}`}>{bmi}</div>
                            <div className="text-gray-600 mt-1">BMI - {bmiCategory.text}</div>
                        </div>
                    </div>

                    {/* Profile Form */}
                    <div className="p-8">
                        <form onSubmit={handleSubmit}>
                            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                                <div>
                                    <label className="block text-gray-700 font-semibold mb-2">First Name</label>
                                    <input
                                        type="text"
                                        name="firstName"
                                        value={formData.firstName}
                                        onChange={handleChange}
                                        disabled={!isEditing}
                                        className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 disabled:bg-gray-100"
                                    />
                                </div>

                                <div>
                                    <label className="block text-gray-700 font-semibold mb-2">Last Name</label>
                                    <input
                                        type="text"
                                        name="lastName"
                                        value={formData.lastName}
                                        onChange={handleChange}
                                        disabled={!isEditing}
                                        className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 disabled:bg-gray-100"
                                    />
                                </div>

                                <div>
                                    <label className="block text-gray-700 font-semibold mb-2">Email</label>
                                    <input
                                        type="email"
                                        value={user.email}
                                        disabled
                                        className="w-full px-4 py-2 border rounded-lg bg-gray-100"
                                    />
                                    <p className="text-sm text-gray-500 mt-1">Email cannot be changed</p>
                                </div>

                                <div>
                                    <label className="block text-gray-700 font-semibold mb-2">Date of Birth</label>
                                    <input
                                        type="text"
                                        value={new Date(user.dateOfBirth).toLocaleDateString()}
                                        disabled
                                        className="w-full px-4 py-2 border rounded-lg bg-gray-100"
                                    />
                                </div>

                                <div>
                                    <label className="block text-gray-700 font-semibold mb-2">Weight (kg)</label>
                                    <input
                                        type="number"
                                        step="0.1"
                                        name="weight"
                                        value={formData.weight}
                                        onChange={handleChange}
                                        disabled={!isEditing}
                                        className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 disabled:bg-gray-100"
                                    />
                                </div>

                                <div>
                                    <label className="block text-gray-700 font-semibold mb-2">Height (cm)</label>
                                    <input
                                        type="number"
                                        step="0.1"
                                        name="height"
                                        value={formData.height}
                                        onChange={handleChange}
                                        disabled={!isEditing}
                                        className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 disabled:bg-gray-100"
                                    />
                                </div>

                                <div>
                                    <label className="block text-gray-700 font-semibold mb-2">Gender</label>
                                    <input
                                        type="text"
                                        value={user.gender}
                                        disabled
                                        className="w-full px-4 py-2 border rounded-lg bg-gray-100"
                                    />
                                </div>

                                <div>
                                    <label className="block text-gray-700 font-semibold mb-2">Fitness Goal</label>
                                    <select
                                        name="fitnessGoal"
                                        value={formData.fitnessGoal}
                                        onChange={handleChange}
                                        disabled={!isEditing}
                                        className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 disabled:bg-gray-100"
                                    >
                                        <option value="Weight Loss">Weight Loss</option>
                                        <option value="Muscle Gain">Muscle Gain</option>
                                        <option value="General Fitness">General Fitness</option>
                                        <option value="Endurance">Endurance</option>
                                    </select>
                                </div>
                            </div>

                            {isEditing && (
                                <div className="flex gap-4 mt-8">
                                    <button
                                        type="button"
                                        onClick={handleCancel}
                                        className="flex-1 bg-gray-300 text-gray-700 py-3 rounded-lg hover:bg-gray-400 font-semibold"
                                    >
                                        Cancel
                                    </button>
                                    <button
                                        type="submit"
                                        disabled={loading}
                                        className="flex-1 bg-blue-500 text-white py-3 rounded-lg hover:bg-blue-600 disabled:bg-gray-400 font-semibold"
                                    >
                                        {loading ? 'Saving...' : 'Save Changes'}
                                    </button>
                                </div>
                            )}
                        </form>
                    </div>

                    {/* Account Info */}
                    <div className="bg-gray-50 p-8 border-t">
                        <h3 className="text-lg font-bold mb-4">Account Information</h3>
                        <div className="grid grid-cols-1 md:grid-cols-2 gap-4 text-sm">
                            <div>
                                <span className="text-gray-600">Member Since:</span>
                                <span className="ml-2 font-semibold">{new Date(user.createdAt || Date.now()).toLocaleDateString()}</span>
                            </div>
                            <div>
                                <span className="text-gray-600">Account Status:</span>
                                <span className="ml-2 font-semibold">{user.isPremium ? 'Premium' : 'Free'}</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Profile;