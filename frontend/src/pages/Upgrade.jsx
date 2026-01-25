import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import api from '../api/axiosConfig';


const Upgrade = () => {
    const { user } = useAuth();
    const navigate = useNavigate();

    const handleUpgrade = async () => {
        try {
            const response = await api.post('/stripe/create-checkout-session', {
                userId: user.id,
                priceId: 'price_1Sss8sDlMwMJ1RUdfPnx4pT9'
            });

            const { url } = response.data;
            window.location.href = url;
        } catch (error) {
            console.error('Error:', error);
            alert('Failed to start checkout. Please try again.');
        }
    };

    return (
        <div className="min-h-screen bg-gradient-to-br from-blue-50 via-white to-purple-50 py-16 px-4">
            <div className="max-w-4xl mx-auto">
                {/* Header */}
                <div className="text-center mb-12">
                    <span className="text-6xl mb-4 block">🌟</span>
                    <h1 className="text-5xl font-bold text-gray-900 mb-4">
                        Upgrade to Premium
                    </h1>
                    <p className="text-xl text-gray-600">
                        Unlock advanced nutrition tracking and meal planning
                    </p>
                </div>

                {/* Pricing Card */}
                <div className="bg-white rounded-3xl shadow-2xl p-8 md:p-12 mb-8">
                    <div className="text-center mb-8">
                        <div className="inline-block bg-gradient-to-r from-blue-600 to-purple-600 text-white px-4 py-2 rounded-full text-sm font-semibold mb-4">
                            MOST POPULAR
                        </div>
                        <h2 className="text-3xl font-bold text-gray-900 mb-2">Premium Plan</h2>
                        <div className="flex items-baseline justify-center gap-2">
                            <span className="text-5xl font-bold text-gray-900">$9.99</span>
                            <span className="text-gray-600 text-xl">/month</span>
                        </div>
                    </div>

                    {/* Features */}
                    <div className="space-y-4 mb-8">
                        <div className="flex items-start gap-4">
                            <div className="bg-green-100 rounded-full p-2 flex-shrink-0">
                                <svg className="w-5 h-5 text-green-600" fill="currentColor" viewBox="0 0 20 20">
                                    <path fillRule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clipRule="evenodd" />
                                </svg>
                            </div>
                            <div>
                                <h3 className="font-semibold text-gray-900 text-lg">Advanced Nutrition Tracking</h3>
                                <p className="text-gray-600">Track macros, calories, and micronutrients with detailed analytics</p>
                            </div>
                        </div>

                        <div className="flex items-start gap-4">
                            <div className="bg-green-100 rounded-full p-2 flex-shrink-0">
                                <svg className="w-5 h-5 text-green-600" fill="currentColor" viewBox="0 0 20 20">
                                    <path fillRule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clipRule="evenodd" />
                                </svg>
                            </div>
                            <div>
                                <h3 className="font-semibold text-gray-900 text-lg">Generated Powered Meal Plans</h3>
                                <p className="text-gray-600">Get personalized meal plans based on your goals and preferences</p>
                            </div>
                        </div>

                        <div className="flex items-start gap-4">
                            <div className="bg-green-100 rounded-full p-2 flex-shrink-0">
                                <svg className="w-5 h-5 text-green-600" fill="currentColor" viewBox="0 0 20 20">
                                    <path fillRule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clipRule="evenodd" />
                                </svg>
                            </div>
                            <div>
                                <h3 className="font-semibold text-gray-900 text-lg">Unlimited Workout Plans</h3>
                                <p className="text-gray-600">Generate unlimited personalized workout plans</p>
                            </div>
                        </div>

                        <div className="flex items-start gap-4">
                            <div className="bg-green-100 rounded-full p-2 flex-shrink-0">
                                <svg className="w-5 h-5 text-green-600" fill="currentColor" viewBox="0 0 20 20">
                                    <path fillRule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clipRule="evenodd" />
                                </svg>
                            </div>
                            <div>
                                <h3 className="font-semibold text-gray-900 text-lg">Progress Analytics</h3>
                                <p className="text-gray-600">Detailed charts and insights on your fitness journey</p>
                            </div>
                        </div>

                        <div className="flex items-start gap-4">
                            <div className="bg-green-100 rounded-full p-2 flex-shrink-0">
                                <svg className="w-5 h-5 text-green-600" fill="currentColor" viewBox="0 0 20 20">
                                    <path fillRule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clipRule="evenodd" />
                                </svg>
                            </div>
                            <div>
                                <h3 className="font-semibold text-gray-900 text-lg">Priority Support</h3>
                                <p className="text-gray-600">Get help faster with priority customer support</p>
                            </div>
                        </div>
                    </div>

                    {/* CTA Button */}
                    <button
                        onClick={handleUpgrade}
                        className="w-full bg-gradient-to-r from-blue-600 to-purple-600 text-white py-5 rounded-xl font-bold text-lg hover:from-blue-700 hover:to-purple-700 transition-all shadow-lg hover:shadow-xl transform hover:scale-105"
                    >
                        Upgrade Now - Start Your Premium Journey
                    </button>

                    <p className="text-center text-gray-500 text-sm mt-4">
                        Cancel anytime. No hidden fees.
                    </p>
                </div>

                {/* Back Button */}
                <div className="text-center">
                    <button
                        onClick={() => navigate('/dashboard')}
                        className="text-gray-600 hover:text-gray-900 font-medium"
                    >
                        ← Back to Dashboard
                    </button>
                </div>
            </div>
        </div>
    );
};

export default Upgrade;