import { motion } from 'framer-motion';

const LoadingSpinner = ({ size = 'md', text = 'Loading...' }) => {
    const sizeClasses = {
        sm: 'h-8 w-8',
        md: 'h-12 w-12',
        lg: 'h-16 w-16',
    };

    const containerVariants = {
        animate: {
            transition: {
                staggerChildren: 0.1,
            },
        },
    };

    const dotVariants = {
        initial: { y: 0, opacity: 0.6 },
        animate: {
            y: -10,
            opacity: 1,
            transition: {
                duration: 0.6,
                repeat: Infinity,
                repeatType: 'reverse',
            },
        },
    };

    return (
        <div className="flex flex-col items-center justify-center gap-4">
            <motion.div
                className="flex gap-2"
                variants={containerVariants}
                initial="initial"
                animate="animate"
            >
                {[0, 1, 2].map((i) => (
                    <motion.div
                        key={i}
                        className={`${sizeClasses[size]} bg-gradient-to-br from-blue-500 to-purple-500 rounded-full`}
                        variants={dotVariants}
                    />
                ))}
            </motion.div>
            {text && (
                <motion.p
                    className="text-gray-600 font-medium"
                    animate={{ opacity: [0.5, 1, 0.5] }}
                    transition={{ duration: 1.5, repeat: Infinity }}
                >
                    {text}
                </motion.p>
            )}
        </div>
    );
};

export default LoadingSpinner;

