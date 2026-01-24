// src/components/SubscriptionButton.jsx
import { useState } from 'react';

function SubscriptionButton({ userId }) {
    const [loading, setLoading] = useState(false);

    const handleSubscribe = async () => {
        setLoading(true);
        try {
            const response = await fetch('http://localhost:5152/api/stripe/create-checkout-session', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    userId: userId,
                    priceId: 'price_1Sss8sDlMwMJ1RUdfPnx4pT9'
                })
            });

            const { url } = await response.json();
            window.location.href = url; // Redirect to Stripe checkout
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