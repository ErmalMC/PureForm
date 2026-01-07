import { useState, useEffect } from 'react';
import { useAuth } from '../context/AuthContext';
import { workoutApi } from '../api/workoutApi';
import { useNavigate } from 'react-router-dom';

const Dashboard = () => {
    const { user, logout } = useAuth();
    const [workoutPlans, setWorkoutPlans] = useState([]);
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();

    useEffect(() => {
        fetchWorkoutPlans();
    }, []);

    const fetchWorkoutPlans = async () => {
        try {
            const plans = await workoutApi.getByUserId(user.id);
            setWorkoutPlans(plans);
        } catch (error) {
            console.error('Error fetching workout plans:', error);
        } finally {
            setLoading(false);
        }
    };

    const handleGeneratePlan = async () => {
        try {
            setLoading(true);
            await workoutApi.generatePersonalized(user.id);
            await fetchWorkoutPlans();
        } catch (error) {
            console.error('Error generating plan:', error);
            alert('Failed to generate personalized plan');
        } finally {
            setLoading(false);
        }
    };

    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    return (
        <div className="min-h-screen bg-gray-100">
            {/* Navbar */}
            <nav className="bg-white shadow-md">
                <div className="max-w-7xl mx-auto px-4 py-4 flex justify-between items-center">
                    <h1 className="text-2xl font-bold text-blue-600">PureForm</h1>
                    <div className="flex items-center gap-4">
                        <span className="text-gray-700">Welcome, {user.firstName}!</span>
                        {user.isPremium && (
                            <span className="bg-yellow-400 text-yellow-900 px-3 py-1 rounded-full text-sm font-semibold">
                Premium
              </span>
                        )}
                        <button
                            onClick={handleLogout}
                            className="bg-red-500 text-white px-4 py-2 rounded-lg hover:bg-red-600"
                        >
                            Logout
                        </button>
                    </div>
                </div>
            </nav>

            {/* Main Content */}
            <div className="max-w-7xl mx-auto px-4 py-8">
                {/* User Stats */}
                <div className="grid grid-cols-1 md:grid-cols-4 gap-4 mb-8">
                    <div className="bg-white p-6 rounded-lg shadow">
                        <h3 className="text-gray-500 text-sm">Weight</h3>
                        <p className="text-2xl font-bold">{user.weight} kg</p>
                    </div>
                    <div className="bg-white p-6 rounded-lg shadow">
                        <h3 className="text-gray-500 text-sm">Height</h3>
                        <p className="text-2xl font-bold">{user.height} cm</p>
                    </div>
                    <div className="bg-white p-6 rounded-lg shadow">
                        <h3 className="text-gray-500 text-sm">Goal</h3>
                        <p className="text-2xl font-bold">{user.fitnessGoal}</p>
                    </div>
                    <div className="bg-white p-6 rounded-lg shadow">
                        <h3 className="text-gray-500 text-sm">Workout Plans</h3>
                        <p className="text-2xl font-bold">{workoutPlans.length}</p>
                    </div>
                </div>

                {/* Workout Plans Section */}
                <div className="bg-white rounded-lg shadow p-6">
                    <div className="flex justify-between items-center mb-6">
                        <h2 className="text-2xl font-bold">My Workout Plans</h2>
                        <button
                            onClick={handleGeneratePlan}
                            disabled={loading}
                            className="bg-blue-500 text-white px-6 py-2 rounded-lg hover:bg-blue-600 disabled:bg-gray-400"
                        >
                            {loading ? 'Generating...' : 'Generate Personalized Plan'}
                        </button>
                    </div>

                    {loading ? (
                        <div className="text-center py-8">
                            <p className="text-gray-500">Loading workout plans...</p>
                        </div>
                    ) : workoutPlans.length === 0 ? (
                        <div className="text-center py-8">
                            <p className="text-gray-500 mb-4">You don't have any workout plans yet.</p>
                            <p className="text-gray-400">Click "Generate Personalized Plan" to get started!</p>
                        </div>
                    ) : (
                        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                            {workoutPlans.map((plan) => (
                                <div key={plan.id} className="border rounded-lg p-4 hover:shadow-lg transition">
                                    <h3 className="text-xl font-bold mb-2">{plan.name}</h3>
                                    <p className="text-gray-600 mb-2">{plan.description}</p>
                                    <div className="flex justify-between items-center text-sm">
                    <span className="bg-blue-100 text-blue-800 px-2 py-1 rounded">
                      {plan.difficultyLevel}
                    </span>
                                        <span className="text-gray-500">{plan.durationWeeks} weeks</span>
                                    </div>
                                    <div className="mt-2 text-sm text-gray-500">
                                        {plan.exercises.length} exercises
                                    </div>
                                </div>
                            ))}
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};

export default Dashboard;