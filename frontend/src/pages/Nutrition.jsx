import { useState, useEffect } from 'react';
import { useAuth } from '../context/AuthContext';
import { nutritionApi } from '../api/nutritionApi';
import { foodApi } from '../api/foodApi';
import { calculatorApi } from '../api/calculatorApi';
import { userApi } from '../api/userApi';
import Navbar from '../components/Navbar';

const Nutrition = () => {
    const { user } = useAuth();
    const [nutritionLogs, setNutritionLogs] = useState([]);
    const [dailyTotals, setDailyTotals] = useState({ Calories: 0, Protein: 0, Carbs: 0, Fats: 0 });
    const [selectedDate, setSelectedDate] = useState(new Date().toISOString().split('T')[0]);
    const [loading, setLoading] = useState(true);
    const [showAddModal, setShowAddModal] = useState(false);
    const [showGoalsModal, setShowGoalsModal] = useState(false);
    const [showFoodSearch, setShowFoodSearch] = useState(false);
    const [popularFoods, setPopularFoods] = useState([]);
    const [searchResults, setSearchResults] = useState([]);
    const [searchQuery, setSearchQuery] = useState('');
    const [recommendations, setRecommendations] = useState(null);
    const [userGoals, setUserGoals] = useState({
        dailyCalorieGoal: user.dailyCalorieGoal || 2000,
        dailyProteinGoal: user.dailyProteinGoal || 150,
        dailyCarbsGoal: user.dailyCarbsGoal || 200,
        dailyFatsGoal: user.dailyFatsGoal || 65
    });
    const [formData, setFormData] = useState({
        logDate: new Date().toISOString().split('T')[0],
        mealType: 'Breakfast',
        foodName: '',
        calories: '',
        protein: '',
        carbs: '',
        fats: '',
        servingSize: '',
        servingUnit: 'g'
    });

    useEffect(() => {
        fetchNutritionLogs();
        loadPopularFoods();
    }, [selectedDate]);

    const loadPopularFoods = async () => {
        try {
            const foods = await foodApi.getAll(); // Use getAll instead of getPopular
            setPopularFoods(foods);
        } catch (error) {
            console.error('Error loading foods:', error);
        }
    };

    const handleFoodSearch = async (query) => {
        setSearchQuery(query);
        if (query.length < 2) {
            setSearchResults([]);
            return;
        }
        try {
            const results = await foodApi.search(query);
            setSearchResults(results);
        } catch (error) {
            console.error('Error searching foods:', error);
        }
    };

    const selectFood = (food, servingSize = 100) => {
        const multiplier = servingSize / 100;
        setFormData({
            ...formData,
            foodName: food.name,
            calories: (food.caloriesPer100g * multiplier).toFixed(1),
            protein: (food.proteinPer100g * multiplier).toFixed(1),
            carbs: (food.carbsPer100g * multiplier).toFixed(1),
            fats: (food.fatsPer100g * multiplier).toFixed(1),
            servingSize: servingSize.toString(),
            servingUnit: food.defaultUnit || 'g'
        });
        setShowFoodSearch(false);
    };

    const loadRecommendations = async () => {
        try {
            const recs = await calculatorApi.getRecommendations(user.id);
            setRecommendations(recs);
        } catch (error) {
            console.error('Error loading recommendations:', error);
        }
    };

    const applyRecommendations = async () => {
        try {
            await calculatorApi.applyRecommendations(user.id);
            const updatedUser = await userApi.getById(user.id);
            localStorage.setItem('user', JSON.stringify(updatedUser));
            setUserGoals({
                dailyCalorieGoal: updatedUser.dailyCalorieGoal,
                dailyProteinGoal: updatedUser.dailyProteinGoal,
                dailyCarbsGoal: updatedUser.dailyCarbsGoal,
                dailyFatsGoal: updatedUser.dailyFatsGoal
            });
            setShowGoalsModal(false);
            alert('Goals updated successfully!');
            window.location.reload();
        } catch (error) {
            console.error('Error applying recommendations:', error);
        }
    };

    const saveCustomGoals = async () => {
        try {
            await userApi.update(user.id, {
                dailyCalorieGoal: parseFloat(userGoals.dailyCalorieGoal),
                dailyProteinGoal: parseFloat(userGoals.dailyProteinGoal),
                dailyCarbsGoal: parseFloat(userGoals.dailyCarbsGoal),
                dailyFatsGoal: parseFloat(userGoals.dailyFatsGoal)
            });
            const updatedUser = await userApi.getById(user.id);
            localStorage.setItem('user', JSON.stringify(updatedUser));
            setShowGoalsModal(false);
            alert('Goals saved successfully!');
            window.location.reload();
        } catch (error) {
            console.error('Error saving goals:', error);
        }
    };

    const fetchNutritionLogs = async () => {
        try {
            setLoading(true);
            const date = new Date(selectedDate);
            const startOfDay = new Date(date.setHours(0, 0, 0, 0)).toISOString();
            const endOfDay = new Date(date.setHours(23, 59, 59, 999)).toISOString();

            const logs = await nutritionApi.getByUserId(user.id, startOfDay, endOfDay);
            setNutritionLogs(logs);

            const totals = await nutritionApi.getDailyTotals(user.id, selectedDate);
            setDailyTotals(totals);
        } catch (error) {
            console.error('Error fetching nutrition logs:', error);
        } finally {
            setLoading(false);
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            await nutritionApi.create(user.id, {
                ...formData,
                calories: parseFloat(formData.calories),
                protein: parseFloat(formData.protein),
                carbs: parseFloat(formData.carbs),
                fats: parseFloat(formData.fats),
                servingSize: parseFloat(formData.servingSize)
            });
            setShowAddModal(false);
            setFormData({
                logDate: new Date().toISOString().split('T')[0],
                mealType: 'Breakfast',
                foodName: '',
                calories: '',
                protein: '',
                carbs: '',
                fats: '',
                servingSize: '',
                servingUnit: 'g'
            });
            await fetchNutritionLogs();
        } catch (error) {
            console.error('Error creating nutrition log:', error);
            alert('Failed to add food');
        }
    };

    const handleDelete = async (id) => {
        if (!confirm('Delete this food entry?')) return;
        try {
            await nutritionApi.delete(id);
            await fetchNutritionLogs();
        } catch (error) {
            console.error('Error deleting nutrition log:', error);
        }
    };

    const getMealIcon = (mealType) => {
        const icons = {
            Breakfast: '🌅',
            Lunch: '☀️',
            Dinner: '🌙',
            Snack: '🍎'
        };
        return icons[mealType] || '🍽️';
    };

    const calorieGoal = userGoals.dailyCalorieGoal || 2000;
    const proteinGoal = userGoals.dailyProteinGoal || 150;
    const carbsGoal = userGoals.dailyCarbsGoal || 200;
    const fatsGoal = userGoals.dailyFatsGoal || 65;

    const caloriePercentage = Math.min((dailyTotals.Calories / calorieGoal) * 100, 100);
    const proteinPercentage = Math.min((dailyTotals.Protein / proteinGoal) * 100, 100);
    const carbsPercentage = Math.min((dailyTotals.Carbs / carbsGoal) * 100, 100);
    const fatsPercentage = Math.min((dailyTotals.Fats / fatsGoal) * 100, 100);

    return (
        <div className="min-h-screen bg-gradient-to-br from-green-50 via-white to-blue-50">
            <Navbar />

            <div className="max-w-7xl mx-auto px-4 py-8">
                {/* Header */}
                <div className="flex justify-between items-center mb-8">
                    <div>
                        <h2 className="text-4xl font-bold text-gray-900 mb-2">Nutrition Tracker</h2>
                        <p className="text-gray-600">Track your daily food intake and macros</p>
                    </div>
                    <div className="flex gap-3">
                        <button
                            onClick={() => setShowGoalsModal(true)}
                            className="bg-blue-500 text-white px-6 py-3 rounded-xl font-semibold hover:bg-blue-600 shadow-lg hover:shadow-xl transform hover:scale-105 transition-all flex items-center gap-2"
                        >
                            <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" />
                            </svg>
                            Set Goals
                        </button>
                        <button
                            onClick={() => setShowAddModal(true)}
                            className="bg-gradient-to-r from-green-500 to-green-600 text-white px-6 py-3 rounded-xl font-semibold hover:from-green-600 hover:to-green-700 shadow-lg hover:shadow-xl transform hover:scale-105 transition-all flex items-center gap-2"
                        >
                            <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" />
                            </svg>
                            Add Food
                        </button>
                    </div>
                </div>

                {/* Date Selector */}
                <div className="bg-white rounded-2xl shadow-lg p-6 mb-8">
                    <div className="flex items-center justify-between">
                        <button
                            onClick={() => {
                                const date = new Date(selectedDate);
                                date.setDate(date.getDate() - 1);
                                setSelectedDate(date.toISOString().split('T')[0]);
                            }}
                            className="p-2 hover:bg-gray-100 rounded-lg transition"
                        >
                            <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 19l-7-7 7-7" />
                            </svg>
                        </button>

                        <input
                            type="date"
                            value={selectedDate}
                            onChange={(e) => setSelectedDate(e.target.value)}
                            className="text-xl font-bold text-gray-900 border-2 border-gray-200 rounded-xl px-4 py-2 focus:outline-none focus:ring-2 focus:ring-green-500"
                        />

                        <button
                            onClick={() => {
                                const date = new Date(selectedDate);
                                date.setDate(date.getDate() + 1);
                                setSelectedDate(date.toISOString().split('T')[0]);
                            }}
                            className="p-2 hover:bg-gray-100 rounded-lg transition"
                        >
                            <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 5l7 7-7 7" />
                            </svg>
                        </button>
                    </div>
                </div>

                {/* Daily Summary */}
                <div className="grid grid-cols-1 md:grid-cols-4 gap-6 mb-8">
                    <div className="bg-white rounded-2xl shadow-lg p-6 relative overflow-hidden">
                        <div className="absolute top-0 right-0 w-32 h-32 bg-gradient-to-br from-orange-100 to-orange-200 rounded-full -mr-16 -mt-16"></div>
                        <div className="relative">
                            <p className="text-gray-600 font-semibold mb-2">Calories</p>
                            <p className="text-3xl font-bold text-orange-600">{Math.round(dailyTotals.Calories)}</p>
                            <div className="mt-4 bg-gray-200 rounded-full h-2 overflow-hidden">
                                <div
                                    className="bg-gradient-to-r from-orange-500 to-orange-600 h-full transition-all"
                                    style={{ width: `${caloriePercentage}%` }}
                                ></div>
                            </div>
                            <p className="text-sm text-gray-500 mt-2">{Math.round(caloriePercentage)}% of {calorieGoal} goal</p>
                        </div>
                    </div>

                    <div className="bg-white rounded-2xl shadow-lg p-6 relative overflow-hidden">
                        <div className="absolute top-0 right-0 w-32 h-32 bg-gradient-to-br from-red-100 to-red-200 rounded-full -mr-16 -mt-16"></div>
                        <div className="relative">
                            <p className="text-gray-600 font-semibold mb-2">Protein</p>
                            <p className="text-3xl font-bold text-red-600">{Math.round(dailyTotals.Protein)}g</p>
                            <div className="mt-4 bg-gray-200 rounded-full h-2 overflow-hidden">
                                <div
                                    className="bg-gradient-to-r from-red-500 to-red-600 h-full transition-all"
                                    style={{ width: `${proteinPercentage}%` }}
                                ></div>
                            </div>
                            <p className="text-sm text-gray-500 mt-2">{Math.round(proteinPercentage)}% of {proteinGoal}g goal</p>
                        </div>
                    </div>

                    <div className="bg-white rounded-2xl shadow-lg p-6 relative overflow-hidden">
                        <div className="absolute top-0 right-0 w-32 h-32 bg-gradient-to-br from-yellow-100 to-yellow-200 rounded-full -mr-16 -mt-16"></div>
                        <div className="relative">
                            <p className="text-gray-600 font-semibold mb-2">Carbs</p>
                            <p className="text-3xl font-bold text-yellow-600">{Math.round(dailyTotals.Carbs)}g</p>
                            <div className="mt-4 bg-gray-200 rounded-full h-2 overflow-hidden">
                                <div
                                    className="bg-gradient-to-r from-yellow-500 to-yellow-600 h-full transition-all"
                                    style={{ width: `${carbsPercentage}%` }}
                                ></div>
                            </div>
                            <p className="text-sm text-gray-500 mt-2">{Math.round(carbsPercentage)}% of {carbsGoal}g goal</p>
                        </div>
                    </div>

                    <div className="bg-white rounded-2xl shadow-lg p-6 relative overflow-hidden">
                        <div className="absolute top-0 right-0 w-32 h-32 bg-gradient-to-br from-purple-100 to-purple-200 rounded-full -mr-16 -mt-16"></div>
                        <div className="relative">
                            <p className="text-gray-600 font-semibold mb-2">Fats</p>
                            <p className="text-3xl font-bold text-purple-600">{Math.round(dailyTotals.Fats)}g</p>
                            <div className="mt-4 bg-gray-200 rounded-full h-2 overflow-hidden">
                                <div
                                    className="bg-gradient-to-r from-purple-500 to-purple-600 h-full transition-all"
                                    style={{ width: `${fatsPercentage}%` }}
                                ></div>
                            </div>
                            <p className="text-sm text-gray-500 mt-2">{Math.round(fatsPercentage)}% of {fatsGoal}g goal</p>
                        </div>
                    </div>
                </div>
                {/* Food Log */}
                <div className="bg-white rounded-2xl shadow-lg p-8">
                    <h3 className="text-2xl font-bold text-gray-900 mb-6">Today's Meals</h3>

                    {loading ? (
                        <div className="text-center py-12">
                            <div className="inline-block animate-spin rounded-full h-12 w-12 border-4 border-green-500 border-t-transparent"></div>
                            <p className="mt-4 text-gray-600">Loading nutrition logs...</p>
                        </div>
                    ) : nutritionLogs.length === 0 ? (
                        <div className="text-center py-12">
                            <div className="inline-flex items-center justify-center w-20 h-20 bg-gray-100 rounded-full mb-4">
                                <svg className="w-10 h-10 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" />
                                </svg>
                            </div>
                            <h4 className="text-lg font-semibold text-gray-900 mb-2">No meals logged yet</h4>
                            <p className="text-gray-500 mb-6">Start tracking your nutrition by adding your first meal!</p>
                            <button
                                onClick={() => setShowAddModal(true)}
                                className="bg-gradient-to-r from-green-500 to-green-600 text-white px-6 py-3 rounded-xl font-semibold hover:from-green-600 hover:to-green-700 shadow-lg"
                            >
                                Add Your First Meal
                            </button>
                        </div>
                    ) : (
                        <div className="space-y-4">
                            {['Breakfast', 'Lunch', 'Dinner', 'Snack'].map((mealType) => {
                                const mealLogs = nutritionLogs.filter(log => log.mealType === mealType);
                                if (mealLogs.length === 0) return null;

                                return (
                                    <div key={mealType} className="border-2 border-gray-200 rounded-xl p-6 hover:border-green-500 transition-all">
                                        <h4 className="text-xl font-bold text-gray-900 mb-4 flex items-center gap-2">
                                            <span className="text-2xl">{getMealIcon(mealType)}</span>
                                            {mealType}
                                            <span className="text-sm font-normal text-gray-500 ml-2">
                                                ({mealLogs.reduce((sum, log) => sum + log.calories, 0).toFixed(0)} cal)
                                            </span>
                                        </h4>
                                        <div className="space-y-3">
                                            {mealLogs.map((log) => (
                                                <div key={log.id} className="flex items-center justify-between bg-gray-50 rounded-lg p-4 hover:bg-gray-100 transition">
                                                    <div className="flex-1">
                                                        <p className="font-semibold text-gray-900">{log.foodName}</p>
                                                        <p className="text-sm text-gray-600">
                                                            {log.servingSize}{log.servingUnit} • {Math.round(log.calories)} cal
                                                        </p>
                                                        <div className="flex gap-4 mt-2 text-xs text-gray-500">
                                                            <span className="bg-red-100 text-red-700 px-2 py-1 rounded">P: {log.protein}g</span>
                                                            <span className="bg-yellow-100 text-yellow-700 px-2 py-1 rounded">C: {log.carbs}g</span>
                                                            <span className="bg-purple-100 text-purple-700 px-2 py-1 rounded">F: {log.fats}g</span>
                                                        </div>
                                                    </div>
                                                    <button
                                                        onClick={() => handleDelete(log.id)}
                                                        className="text-red-500 hover:text-red-700 hover:bg-red-50 p-2 rounded-lg transition"
                                                    >
                                                        <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                                                        </svg>
                                                    </button>
                                                </div>
                                            ))}
                                        </div>
                                    </div>
                                );
                            })}
                        </div>
                    )}
                </div>

                {/* Add Food Modal */}
                {showAddModal && (
                    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
                        <div className="bg-white rounded-2xl p-8 max-w-2xl w-full max-h-[90vh] overflow-y-auto">
                            <h3 className="text-2xl font-bold mb-6">Add Food Entry</h3>
                            <form onSubmit={handleSubmit} className="space-y-5">
                                <div className="grid grid-cols-2 gap-4">
                                    <div>
                                        <label className="block text-sm font-semibold text-gray-700 mb-2">Date</label>
                                        <input
                                            type="date"
                                            value={formData.logDate}
                                            onChange={(e) => setFormData({...formData, logDate: e.target.value})}
                                            className="w-full px-4 py-3 bg-gray-50 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-green-500"
                                            required
                                        />
                                    </div>
                                    <div>
                                        <label className="block text-sm font-semibold text-gray-700 mb-2">Meal Type</label>
                                        <select
                                            value={formData.mealType}
                                            onChange={(e) => setFormData({...formData, mealType: e.target.value})}
                                            className="w-full px-4 py-3 bg-gray-50 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-green-500"
                                        >
                                            <option value="Breakfast">🌅 Breakfast</option>
                                            <option value="Lunch">☀️ Lunch</option>
                                            <option value="Dinner">🌙 Dinner</option>
                                            <option value="Snack">🍎 Snack</option>
                                        </select>
                                    </div>
                                </div>

                                <div>
                                    <label className="block text-sm font-semibold text-gray-700 mb-2">Food Name</label>
                                    <div className="flex gap-2">
                                        <input
                                            type="text"
                                            value={formData.foodName}
                                            onChange={(e) => setFormData({...formData, foodName: e.target.value})}
                                            className="flex-1 px-4 py-3 bg-gray-50 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-green-500"
                                            placeholder="e.g., Grilled Chicken Breast"
                                            required
                                        />
                                        <button
                                            type="button"
                                            onClick={() => setShowFoodSearch(true)}
                                            className="px-4 py-3 bg-blue-500 text-white rounded-xl hover:bg-blue-600 transition-all whitespace-nowrap"
                                        >
                                            🔍 Search
                                        </button>
                                    </div>
                                </div>

                                <div className="grid grid-cols-2 gap-4">
                                    <div>
                                        <label className="block text-sm font-semibold text-gray-700 mb-2">Serving Size</label>
                                        <input
                                            type="number"
                                            step="0.1"
                                            value={formData.servingSize}
                                            onChange={(e) => setFormData({...formData, servingSize: e.target.value})}
                                            className="w-full px-4 py-3 bg-gray-50 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-green-500"
                                            required
                                        />
                                    </div>
                                    <div>
                                        <label className="block text-sm font-semibold text-gray-700 mb-2">Unit</label>
                                        <select
                                            value={formData.servingUnit}
                                            onChange={(e) => setFormData({...formData, servingUnit: e.target.value})}
                                            className="w-full px-4 py-3 bg-gray-50 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-green-500"
                                        >
                                            <option value="g">grams (g)</option>
                                            <option value="oz">ounces (oz)</option>
                                            <option value="cup">cup</option>
                                            <option value="tbsp">tablespoon</option>
                                            <option value="piece">piece</option>
                                        </select>
                                    </div>
                                </div>

                                <div className="grid grid-cols-2 gap-4">
                                    <div>
                                        <label className="block text-sm font-semibold text-gray-700 mb-2">Calories</label>
                                        <input
                                            type="number"
                                            step="0.1"
                                            value={formData.calories}
                                            onChange={(e) => setFormData({...formData, calories: e.target.value})}
                                            className="w-full px-4 py-3 bg-gray-50 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-green-500"
                                            required
                                        />
                                    </div>
                                    <div>
                                        <label className="block text-sm font-semibold text-gray-700 mb-2">Protein (g)</label>
                                        <input
                                            type="number"
                                            step="0.1"
                                            value={formData.protein}
                                            onChange={(e) => setFormData({...formData, protein: e.target.value})}
                                            className="w-full px-4 py-3 bg-gray-50 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-green-500"
                                            required
                                        />
                                    </div>
                                </div>

                                <div className="grid grid-cols-2 gap-4">
                                    <div>
                                        <label className="block text-sm font-semibold text-gray-700 mb-2">Carbs (g)</label>
                                        <input
                                            type="number"
                                            step="0.1"
                                            value={formData.carbs}
                                            onChange={(e) => setFormData({...formData, carbs: e.target.value})}
                                            className="w-full px-4 py-3 bg-gray-50 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-green-500"
                                            required
                                        />
                                    </div>
                                    <div>
                                        <label className="block text-sm font-semibold text-gray-700 mb-2">Fats (g)</label>
                                        <input
                                            type="number"
                                            step="0.1"
                                            value={formData.fats}
                                            onChange={(e) => setFormData({...formData, fats: e.target.value})}
                                            className="w-full px-4 py-3 bg-gray-50 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-green-500"
                                            required
                                        />
                                    </div>
                                </div>

                                <div className="flex gap-4 mt-6">
                                    <button
                                        type="button"
                                        onClick={() => setShowAddModal(false)}
                                        className="flex-1 bg-gray-200 text-gray-700 py-3 rounded-xl font-semibold hover:bg-gray-300 transition-all"
                                    >
                                        Cancel
                                    </button>
                                    <button
                                        type="submit"
                                        className="flex-1 bg-gradient-to-r from-green-500 to-green-600 text-white py-3 rounded-xl font-semibold hover:from-green-600 hover:to-green-700 transition-all shadow-lg"
                                    >
                                        Add Food
                                    </button>
                                </div>
                            </form>
                        </div>
                    </div>
                )}

                {/* Food Search Modal */}
                {showFoodSearch && (
                    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
                        <div className="bg-white rounded-2xl p-8 max-w-4xl w-full max-h-[90vh] overflow-y-auto">
                            <div className="flex justify-between items-center mb-6">
                                <h3 className="text-2xl font-bold">Search Foods</h3>
                                <button
                                    onClick={() => setShowFoodSearch(false)}
                                    className="text-gray-500 hover:text-gray-700"
                                >
                                    <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
                                    </svg>
                                </button>
                            </div>

                            <div className="mb-6">
                                <input
                                    type="text"
                                    placeholder="Search for foods..."
                                    value={searchQuery}
                                    onChange={(e) => handleFoodSearch(e.target.value)}
                                    className="w-full px-4 py-3 bg-gray-50 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-green-500"
                                    autoFocus
                                />
                            </div>

                            {searchQuery.length >= 2 && (
                                <div className="mb-6">
                                    <h4 className="text-lg font-semibold mb-4">Search Results</h4>
                                    {searchResults.length === 0 ? (
                                        <p className="text-gray-500 text-center py-4">No results found</p>
                                    ) : (
                                        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                                            {searchResults.map((food) => (
                                                <div
                                                    key={food.id}
                                                    onClick={() => selectFood(food)}
                                                    className="border-2 border-gray-200 rounded-xl p-4 hover:border-green-500 cursor-pointer transition-all"
                                                >
                                                    <h5 className="font-semibold text-gray-900 mb-2">{food.name}</h5>
                                                    <p className="text-sm text-gray-600 mb-2">{food.category}</p>
                                                    <div className="flex gap-2 text-xs">
                                                        <span className="bg-orange-100 text-orange-700 px-2 py-1 rounded">{food.caloriesPer100g} cal</span>
                                                        <span className="bg-red-100 text-red-700 px-2 py-1 rounded">P: {food.proteinPer100g}g</span>
                                                        <span className="bg-yellow-100 text-yellow-700 px-2 py-1 rounded">C: {food.carbsPer100g}g</span>
                                                        <span className="bg-purple-100 text-purple-700 px-2 py-1 rounded">F: {food.fatsPer100g}g</span>
                                                    </div>
                                                </div>
                                            ))}
                                        </div>
                                    )}
                                </div>
                            )}

                            <div>
                                <h4 className="text-lg font-semibold mb-4">Popular Foods</h4>
                                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                                    {popularFoods.map((food) => (
                                        <div
                                            key={food.id}
                                            onClick={() => selectFood(food)}
                                            className="border-2 border-gray-200 rounded-xl p-4 hover:border-green-500 cursor-pointer transition-all"
                                        >
                                            <h5 className="font-semibold text-gray-900 mb-2">{food.name}</h5>
                                            <p className="text-sm text-gray-600 mb-2">{food.category}</p>
                                            <div className="flex gap-2 text-xs">
                                                <span className="bg-orange-100 text-orange-700 px-2 py-1 rounded">{food.caloriesPer100g} cal</span>
                                                <span className="bg-red-100 text-red-700 px-2 py-1 rounded">P: {food.proteinPer100g}g</span>
                                                <span className="bg-yellow-100 text-yellow-700 px-2 py-1 rounded">C: {food.carbsPer100g}g</span>
                                                <span className="bg-purple-100 text-purple-700 px-2 py-1 rounded">F: {food.fatsPer100g}g</span>
                                            </div>
                                        </div>
                                    ))}
                                </div>
                            </div>
                        </div>
                    </div>
                )}

                {/* Goals Modal */}
                {showGoalsModal && (
                    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
                        <div className="bg-white rounded-2xl p-8 max-w-2xl w-full max-h-[90vh] overflow-y-auto">
                            <h3 className="text-2xl font-bold mb-6">Set Your Daily Goals</h3>

                            <div className="mb-6 p-4 bg-blue-50 rounded-xl">
                                <h4 className="font-semibold text-blue-900 mb-3">Get Recommendations</h4>
                                <p className="text-sm text-blue-700 mb-4">
                                    We can calculate personalized nutrition goals based on your profile, fitness goals, and activity level.
                                </p>
                                <button
                                    onClick={loadRecommendations}
                                    className="w-full bg-blue-500 text-white py-2 rounded-lg hover:bg-blue-600 transition-all"
                                >
                                    Calculate Recommendations
                                </button>

                                {recommendations && (
                                    <div className="mt-4 p-4 bg-white rounded-lg">
                                        <p className="font-semibold mb-2">Recommended Goals:</p>
                                        <div className="grid grid-cols-2 gap-2 text-sm">
                                            <div>Calories: <span className="font-bold">{Math.round(recommendations.recommendedCalories)}</span></div>
                                            <div>Protein: <span className="font-bold">{Math.round(recommendations.recommendedProtein)}g</span></div>
                                            <div>Carbs: <span className="font-bold">{Math.round(recommendations.recommendedCarbs)}g</span></div>
                                            <div>Fats: <span className="font-bold">{Math.round(recommendations.recommendedFats)}g</span></div>
                                        </div>
                                        <button
                                            onClick={applyRecommendations}
                                            className="w-full mt-3 bg-green-500 text-white py-2 rounded-lg hover:bg-green-600 transition-all"
                                        >
                                            Apply These Goals
                                        </button>
                                    </div>
                                )}
                            </div>

                            <div>
                                <h4 className="font-semibold mb-4">Or Set Custom Goals</h4>
                                <div className="space-y-4">
                                    <div>
                                        <label className="block text-sm font-semibold text-gray-700 mb-2">Daily Calorie Goal</label>
                                        <input
                                            type="number"
                                            value={userGoals.dailyCalorieGoal}
                                            onChange={(e) => setUserGoals({...userGoals, dailyCalorieGoal: e.target.value})}
                                            className="w-full px-4 py-3 bg-gray-50 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-green-500"
                                        />
                                    </div>
                                    <div>
                                        <label className="block text-sm font-semibold text-gray-700 mb-2">Daily Protein Goal (g)</label>
                                        <input
                                            type="number"
                                            value={userGoals.dailyProteinGoal}
                                            onChange={(e) => setUserGoals({...userGoals, dailyProteinGoal: e.target.value})}
                                            className="w-full px-4 py-3 bg-gray-50 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-green-500"
                                        />
                                    </div>
                                    <div>
                                        <label className="block text-sm font-semibold text-gray-700 mb-2">Daily Carbs Goal (g)</label>
                                        <input
                                            type="number"
                                            value={userGoals.dailyCarbsGoal}
                                            onChange={(e) => setUserGoals({...userGoals, dailyCarbsGoal: e.target.value})}
                                            className="w-full px-4 py-3 bg-gray-50 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-green-500"
                                        />
                                    </div>
                                    <div>
                                        <label className="block text-sm font-semibold text-gray-700 mb-2">Daily Fats Goal (g)</label>
                                        <input
                                            type="number"
                                            value={userGoals.dailyFatsGoal}
                                            onChange={(e) => setUserGoals({...userGoals, dailyFatsGoal: e.target.value})}
                                            className="w-full px-4 py-3 bg-gray-50 border border-gray-300 rounded-xl focus:outline-none focus:ring-2 focus:ring-green-500"
                                        />
                                    </div>
                                </div>
                            </div>

                            <div className="flex gap-4 mt-6">
                                <button
                                    onClick={() => setShowGoalsModal(false)}
                                    className="flex-1 bg-gray-200 text-gray-700 py-3 rounded-xl font-semibold hover:bg-gray-300 transition-all"
                                >
                                    Cancel
                                </button>
                                <button
                                    onClick={saveCustomGoals}
                                    className="flex-1 bg-gradient-to-r from-green-500 to-green-600 text-white py-3 rounded-xl font-semibold hover:from-green-600 hover:to-green-700 transition-all"
                                >
                                    Save Goals
                                </button>
                            </div>
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
};

export default Nutrition;