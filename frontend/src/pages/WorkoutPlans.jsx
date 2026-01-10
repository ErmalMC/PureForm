// ============================================================
// src/pages/WorkoutPlans.jsx
// ============================================================
import { useState, useEffect } from 'react';
import { useAuth } from '../context/AuthContext';
import { workoutApi } from '../api/workoutApi';
import Navbar from '../components/Navbar';

const WorkoutPlans = () => {
    const { user } = useAuth();
    const [workoutPlans, setWorkoutPlans] = useState([]);
    const [loading, setLoading] = useState(true);
    const [showCreateModal, setShowCreateModal] = useState(false);
    const [selectedPlan, setSelectedPlan] = useState(null);
    const [formData, setFormData] = useState({
        name: '',
        description: '',
        difficultyLevel: 'Beginner',
        durationWeeks: 4
    });

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

    const handleCreatePlan = async (e) => {
        e.preventDefault();
        try {
            setLoading(true);
            await workoutApi.create(user.id, formData);
            setShowCreateModal(false);
            setFormData({
                name: '',
                description: '',
                difficultyLevel: 'Beginner',
                durationWeeks: 4
            });
            await fetchWorkoutPlans();
        } catch (error) {
            console.error('Error creating workout plan:', error);
            alert('Failed to create workout plan');
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

    const handleDeletePlan = async (id) => {
        if (!confirm('Are you sure you want to delete this workout plan?')) return;

        try {
            await workoutApi.delete(id);
            await fetchWorkoutPlans();
        } catch (error) {
            console.error('Error deleting plan:', error);
            alert('Failed to delete workout plan');
        }
    };

    return (
        <div className="min-h-screen bg-gradient-to-br from-blue-50 via-white to-purple-50">
            <Navbar />

            {/* Main Content */}
            <div className="max-w-7xl mx-auto px-4 py-8">
                <div className="flex justify-between items-center mb-8">
                    <div>
                        <h2 className="text-4xl font-bold text-gray-900 mb-2">My Workout Plans</h2>
                        <p className="text-gray-600">Create and manage your personalized workout routines</p>
                    </div>
                    <div className="flex gap-4">
                        <button
                            onClick={handleGeneratePlan}
                            disabled={loading}
                            className="bg-gradient-to-r from-green-500 to-green-600 text-white px-6 py-3 rounded-xl font-semibold hover:from-green-600 hover:to-green-700 disabled:opacity-50 shadow-lg hover:shadow-xl transform hover:scale-105 transition-all flex items-center gap-2"
                        >
                            <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9.663 17h4.673M12 3v1m6.364 1.636l-.707.707M21 12h-1M4 12H3m3.343-5.657l-.707-.707m2.828 9.9a5 5 0 117.072 0l-.548.547A3.374 3.374 0 0014 18.469V19a2 2 0 11-4 0v-.531c0-.895-.356-1.754-.988-2.386l-.548-.547z" />
                            </svg>
                            Generate AI Plan
                        </button>
                        <button
                            onClick={() => setShowCreateModal(true)}
                            className="bg-gradient-to-r from-blue-600 to-purple-600 text-white px-6 py-3 rounded-xl font-semibold hover:from-blue-700 hover:to-purple-700 shadow-lg hover:shadow-xl transform hover:scale-105 transition-all flex items-center gap-2"
                        >
                            <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" />
                            </svg>
                            Create New Plan
                        </button>
                    </div>
                </div>

                {loading ? (
                    <div className="text-center py-12">
                        <div className="inline-block animate-spin rounded-full h-16 w-16 border-4 border-blue-500 border-t-transparent"></div>
                        <p className="mt-4 text-gray-600 font-medium">Loading your workout plans...</p>
                    </div>
                ) : workoutPlans.length === 0 ? (
                    <div className="bg-white rounded-2xl shadow-xl p-12 text-center">
                        <div className="inline-flex items-center justify-center w-24 h-24 bg-gradient-to-br from-blue-100 to-purple-100 rounded-full mb-6">
                            <svg className="w-12 h-12 text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 012-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10" />
                            </svg>
                        </div>
                        <h3 className="text-2xl font-bold text-gray-900 mb-3">No workout plans yet</h3>
                        <p className="text-gray-500 mb-8 max-w-md mx-auto">Start your fitness journey by creating a personalized workout plan or let our AI generate one for you!</p>
                        <div className="flex gap-4 justify-center">
                            <button
                                onClick={handleGeneratePlan}
                                className="bg-gradient-to-r from-green-500 to-green-600 text-white px-6 py-3 rounded-xl font-semibold hover:from-green-600 hover:to-green-700 shadow-lg"
                            >
                                Generate AI Plan
                            </button>
                            <button
                                onClick={() => setShowCreateModal(true)}
                                className="bg-gradient-to-r from-blue-600 to-purple-600 text-white px-6 py-3 rounded-xl font-semibold hover:from-blue-700 hover:to-purple-700 shadow-lg"
                            >
                                Create Manual Plan
                            </button>
                        </div>
                    </div>
                ) : (
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                        {workoutPlans.map((plan) => (
                            <div key={plan.id} className="group bg-white rounded-2xl shadow-lg overflow-hidden hover:shadow-2xl transition-all transform hover:scale-105 border-2 border-gray-100 hover:border-blue-500">
                                <div className="p-6">
                                    <div className="flex justify-between items-start mb-4">
                                        <h3 className="text-xl font-bold text-gray-900 group-hover:text-blue-600 transition-colors">{plan.name}</h3>
                                        <button
                                            onClick={() => handleDeletePlan(plan.id)}
                                            className="text-red-400 hover:text-red-600 hover:bg-red-50 p-2 rounded-lg transition-all"
                                        >
                                            <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                                            </svg>
                                        </button>
                                    </div>

                                    <p className="text-gray-600 mb-4 line-clamp-2 min-h-[3rem]">{plan.description}</p>

                                    <div className="flex items-center gap-2 mb-4">
                    <span className={`px-3 py-1 rounded-full text-sm font-bold ${
                        plan.difficultyLevel === 'Beginner' ? 'bg-green-100 text-green-700' :
                            plan.difficultyLevel === 'Intermediate' ? 'bg-yellow-100 text-yellow-700' :
                                'bg-red-100 text-red-700'
                    }`}>
                      {plan.difficultyLevel}
                    </span>
                                        <span className="px-3 py-1 bg-blue-100 text-blue-700 rounded-full text-sm font-bold">{plan.durationWeeks} weeks</span>
                                    </div>

                                    <div className="pt-4 border-t border-gray-200">
                                        <div className="flex items-center text-sm text-gray-600 font-medium mb-4">
                                            <svg className="w-4 h-4 mr-2 text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M13 10V3L4 14h7v7l9-11h-7z" />
                                            </svg>
                                            {plan.exercises.length} exercises included
                                        </div>
                                    </div>

                                    <button
                                        onClick={() => setSelectedPlan(plan)}
                                        className="w-full bg-gradient-to-r from-blue-600 to-purple-600 text-white py-3 rounded-xl font-semibold hover:from-blue-700 hover:to-purple-700 transition-all shadow-lg"
                                    >
                                        View Details
                                    </button>
                                </div>
                            </div>
                        ))}
                    </div>
                )}

                {/* Create Modal */}
                {showCreateModal && (
                    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
                        <div className="bg-white rounded-lg p-8 max-w-md w-full mx-4">
                            <h3 className="text-2xl font-bold mb-6">Create Workout Plan</h3>
                            <form onSubmit={handleCreatePlan}>
                                <div className="mb-4">
                                    <label className="block text-gray-700 mb-2">Plan Name</label>
                                    <input
                                        type="text"
                                        value={formData.name}
                                        onChange={(e) => setFormData({...formData, name: e.target.value})}
                                        className="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                                        required
                                    />
                                </div>
                                <div className="mb-4">
                                    <label className="block text-gray-700 mb-2">Description</label>
                                    <textarea
                                        value={formData.description}
                                        onChange={(e) => setFormData({...formData, description: e.target.value})}
                                        className="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                                        rows="3"
                                        required
                                    />
                                </div>
                                <div className="mb-4">
                                    <label className="block text-gray-700 mb-2">Difficulty Level</label>
                                    <select
                                        value={formData.difficultyLevel}
                                        onChange={(e) => setFormData({...formData, difficultyLevel: e.target.value})}
                                        className="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                                    >
                                        <option value="Beginner">Beginner</option>
                                        <option value="Intermediate">Intermediate</option>
                                        <option value="Advanced">Advanced</option>
                                    </select>
                                </div>
                                <div className="mb-6">
                                    <label className="block text-gray-700 mb-2">Duration (weeks)</label>
                                    <input
                                        type="number"
                                        min="1"
                                        max="52"
                                        value={formData.durationWeeks}
                                        onChange={(e) => setFormData({...formData, durationWeeks: parseInt(e.target.value)})}
                                        className="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                                        required
                                    />
                                </div>
                                <div className="flex gap-4">
                                    <button
                                        type="button"
                                        onClick={() => setShowCreateModal(false)}
                                        className="flex-1 bg-gray-300 text-gray-700 py-2 rounded-lg hover:bg-gray-400"
                                    >
                                        Cancel
                                    </button>
                                    <button
                                        type="submit"
                                        className="flex-1 bg-blue-500 text-white py-2 rounded-lg hover:bg-blue-600"
                                    >
                                        Create
                                    </button>
                                </div>
                            </form>
                        </div>
                    </div>
                )}

                {/* View Details Modal */}
                {selectedPlan && (
                    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
                        <div className="bg-white rounded-lg p-8 max-w-2xl w-full max-h-[90vh] overflow-y-auto">
                            <div className="flex justify-between items-start mb-6">
                                <h3 className="text-2xl font-bold">{selectedPlan.name}</h3>
                                <button
                                    onClick={() => setSelectedPlan(null)}
                                    className="text-gray-500 hover:text-gray-700"
                                >
                                    <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
                                    </svg>
                                </button>
                            </div>

                            <p className="text-gray-600 mb-4">{selectedPlan.description}</p>

                            <div className="flex gap-4 mb-6">
                <span className={`px-3 py-1 rounded-full text-sm font-semibold ${
                    selectedPlan.difficultyLevel === 'Beginner' ? 'bg-green-100 text-green-800' :
                        selectedPlan.difficultyLevel === 'Intermediate' ? 'bg-yellow-100 text-yellow-800' :
                            'bg-red-100 text-red-800'
                }`}>
                  {selectedPlan.difficultyLevel}
                </span>
                                <span className="px-3 py-1 bg-blue-100 text-blue-800 rounded-full text-sm font-semibold">
                  {selectedPlan.durationWeeks} weeks
                </span>
                            </div>

                            <h4 className="text-xl font-bold mb-4">Exercises ({selectedPlan.exercises.length})</h4>

                            {selectedPlan.exercises.length > 0 ? (
                                <div className="space-y-4">
                                    {selectedPlan.exercises.map((exercise, index) => (
                                        <div key={exercise.id} className="border rounded-lg p-4">
                                            <div className="flex items-start justify-between">
                                                <div className="flex-1">
                                                    <h5 className="font-semibold text-lg">{index + 1}. {exercise.name}</h5>
                                                    <p className="text-gray-600 text-sm mt-1">{exercise.description}</p>
                                                    <div className="mt-2 flex flex-wrap gap-2 text-sm">
                            <span className="bg-purple-100 text-purple-800 px-2 py-1 rounded">
                              {exercise.muscleGroup}
                            </span>
                                                        <span className="bg-gray-100 text-gray-800 px-2 py-1 rounded">
                              {exercise.equipment}
                            </span>
                                                        <span className="bg-blue-100 text-blue-800 px-2 py-1 rounded">
                              {exercise.sets} sets × {exercise.reps} reps
                            </span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    ))}
                                </div>
                            ) : (
                                <p className="text-gray-500 text-center py-8">No exercises added yet</p>
                            )}
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
};

export default WorkoutPlans;