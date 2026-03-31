import { motion } from 'framer-motion';

const StatCard = ({ 
    title, 
    value, 
    icon: Icon, 
    color = 'blue',
    trend = null,
    trendUp = true,
    delay = 0 
}) => {
    const colorClasses = {
        blue: 'from-blue-500 to-blue-600',
        purple: 'from-purple-500 to-purple-600',
        green: 'from-green-500 to-green-600',
        yellow: 'from-yellow-500 to-yellow-600',
        red: 'from-red-500 to-red-600',
        pink: 'from-pink-500 to-pink-600',
    };

    const bgColorClasses = {
        blue: 'from-blue-50 to-blue-100',
        purple: 'from-purple-50 to-purple-100',
        green: 'from-green-50 to-green-100',
        yellow: 'from-yellow-50 to-yellow-100',
        red: 'from-red-50 to-red-100',
        pink: 'from-pink-50 to-pink-100',
    };

    return (
        <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.4, delay }}
            whileHover={{ y: -8, boxShadow: '0 20px 25px -5px rgba(0, 0, 0, 0.15)' }}
            className="group"
        >
            <div className={`bg-gradient-to-br ${colorClasses[color]} rounded-2xl shadow-lg p-6 text-white overflow-hidden relative`}>
                {/* Background blur effect */}
                <div className="absolute inset-0 opacity-0 group-hover:opacity-10 bg-white transition-opacity duration-300" />

                <div className="relative flex items-start justify-between">
                    <div className="flex-1">
                        <motion.p
                            className="text-white/80 text-sm font-medium mb-2"
                            initial={{ opacity: 0 }}
                            animate={{ opacity: 1 }}
                            transition={{ delay: delay + 0.1 }}
                        >
                            {title}
                        </motion.p>

                        <motion.div
                            initial={{ scale: 0.8, opacity: 0 }}
                            animate={{ scale: 1, opacity: 1 }}
                            transition={{ delay: delay + 0.2, type: 'spring' }}
                        >
                            <p className="text-4xl font-bold">{value}</p>
                        </motion.div>

                        {trend && (
                            <motion.div
                                className={`flex items-center gap-1 mt-2 text-sm font-semibold ${
                                    trendUp ? 'text-green-200' : 'text-red-200'
                                }`}
                                initial={{ opacity: 0, x: -10 }}
                                animate={{ opacity: 1, x: 0 }}
                                transition={{ delay: delay + 0.3 }}
                            >
                                <svg
                                    className={`w-4 h-4 ${trendUp ? '' : 'rotate-180'}`}
                                    fill="none"
                                    stroke="currentColor"
                                    viewBox="0 0 24 24"
                                >
                                    <path
                                        strokeLinecap="round"
                                        strokeLinejoin="round"
                                        strokeWidth={2}
                                        d="M7 16V4m0 0L3 8m0 0l4 4m10 0v12m0 0l4-4m0 0l-4-4"
                                    />
                                </svg>
                                {trend}
                            </motion.div>
                        )}
                    </div>

                    {Icon && (
                        <motion.div
                            className={`bg-gradient-to-br ${bgColorClasses[color]} p-3 rounded-xl ml-4`}
                            initial={{ scale: 0, rotate: -180 }}
                            animate={{ scale: 1, rotate: 0 }}
                            transition={{ delay: delay + 0.2, type: 'spring' }}
                            whileHover={{ scale: 1.1, rotate: 10 }}
                        >
                            <Icon className="w-8 h-8 text-white" />
                        </motion.div>
                    )}
                </div>
            </div>
        </motion.div>
    );
};

export default StatCard;

