import { useState } from 'react';
import api from '../api/axiosConfig';

function SubscriptionButton({ userId }) {
    const [loading, setLoading] = useState(false);

    const handleSubscribe = async () => {
        setLoading(true);
        try {
            const response = await api.post('/stripe/create-checkout-session', {
                userId: userId,
                priceId: 'price_1Sss8sDlMwMJ1RUdfPnx4pT9'
            });

            const { url } = response.data;
            window.location.href = url;
        } catch (error) {
            console.error('Error:', error);
            alert('Failed to start checkout');
        } finally {
            setLoading(false);
        }
    };

    return (
        <button onClick={handleSubscribe} disabled={loading}>
            {loading ? 'Loading...' : 'Subscribe to Premium'}
        </button>
    );
}

export default SubscriptionButton;