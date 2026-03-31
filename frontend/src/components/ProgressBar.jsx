import { motion } from 'framer-motion';

const ProgressBar = ({ value, max = 100, label = null, color = 'blue', animated = true }) => {
    const percentage = (value / max) * 100;

    const colorClasses = {
        blue: 'from-blue-500 to-blue-600',
        purple: 'from-purple-500 to-purple-600',
        green: 'from-green-500 to-green-600',
        yellow: 'from-yellow-500 to-yellow-600',
        red: 'from-red-500 to-red-600',
    };

    return (
        <motion.div
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            transition={{ duration: 0.3 }}
        >
            {label && (
                <div className="flex justify-between items-center mb-2">
                    <label className="text-sm font-semibold text-gray-700">{label}</label>
                    <span className="text-sm font-bold text-gray-600">
                        {value}/{max}
                    </span>
                </div>
            )}

            <div className="w-full h-3 bg-gray-200 rounded-full overflow-hidden shadow-sm">
                <motion.div
                    className={`h-full bg-gradient-to-r ${colorClasses[color]} rounded-full`}
                    initial={{ width: 0 }}
                    animate={{ width: `${percentage}%` }}
                    transition={{
                        duration: animated ? 1 : 0.3,
                        ease: 'easeInOut',
                    }}
                >
                    {animated && (
                        <motion.div
                            className="h-full bg-white/30"
                            animate={{ x: ['-100%', '100%'] }}
                            transition={{ duration: 1.5, repeat: Infinity, ease: 'linear' }}
                        />
                    )}
                </motion.div>
            </div>

            {percentage >= 100 && (
                <motion.div
                    className="text-green-500 text-sm font-semibold mt-2 flex items-center gap-1"
                    initial={{ opacity: 0, scale: 0.8 }}
                    animate={{ opacity: 1, scale: 1 }}
                    transition={{ duration: 0.3 }}
                >
                    <svg className="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
                        <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clipRule="evenodd" />
                    </svg>
                    Goal reached!
                </motion.div>
            )}
        </motion.div>
    );
};

export default ProgressBar;

