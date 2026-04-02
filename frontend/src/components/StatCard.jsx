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
    const iconColorClasses = {
        blue: 'text-blue-700',
        purple: 'text-purple-700',
        green: 'text-green-700',
        yellow: 'text-yellow-700',
        red: 'text-red-700',
        pink: 'text-pink-700',
    };

    const iconBgColorClasses = {
        blue: 'bg-blue-100 border-blue-200',
        purple: 'bg-purple-100 border-purple-200',
        green: 'bg-green-100 border-green-200',
        yellow: 'bg-yellow-100 border-yellow-200',
        red: 'bg-red-100 border-red-200',
        pink: 'bg-pink-100 border-pink-200',
    };

    return (
        <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.4, delay }}
            whileHover={{ y: -4 }}
            className="h-full"
        >
            <div className="bg-white border border-slate-200 rounded-2xl shadow-sm p-6 h-full min-h-37">
                <div className="flex items-start justify-between gap-4 h-full">
                    <div className="flex-1">
                        <motion.p
                            className="text-slate-500 text-sm font-semibold mb-2"
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
                            <p className="text-3xl md:text-[1.9rem] leading-tight font-bold text-slate-900 wrap-break-word">{value}</p>
                        </motion.div>

                        {trend && (
                            <motion.div
                                className={`flex items-center gap-1 mt-2 text-sm font-semibold ${
                                    trendUp ? 'text-green-600' : 'text-red-600'
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
                            className={`p-3 rounded-xl border ${iconBgColorClasses[color]} ${iconColorClasses[color]} shrink-0`}
                            initial={{ scale: 0, rotate: -180 }}
                            animate={{ scale: 1, rotate: 0 }}
                            transition={{ delay: delay + 0.2, type: 'spring' }}
                            whileHover={{ y: -1 }}
                        >
                            <Icon className="w-7 h-7" />
                        </motion.div>
                    )}
                </div>
            </div>
        </motion.div>
    );
};

export default StatCard;

