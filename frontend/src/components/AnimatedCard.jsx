import { motion } from 'framer-motion';

const AnimatedCard = ({
    children,
    className = '',
    variant = 'default',
    isHoverable = true,
    delay = 0,
    icon: Icon = null,
    title = null,
    subtitle = null,
    footer = null,
    onClick = null,
}) => {
    const variantClasses = {
        default: 'bg-white rounded-2xl shadow-lg p-6',
        gradient: 'bg-gradient-to-br from-blue-50 to-purple-50 rounded-2xl shadow-lg p-6',
        dark: 'bg-gray-900 text-white rounded-2xl shadow-xl p-6',
    };

    return (
        <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.4, delay }}
            whileHover={isHoverable ? { y: -4, boxShadow: '0 20px 25px -5px rgba(0, 0, 0, 0.2)' } : {}}
            onClick={onClick}
            className={`${variantClasses[variant]} ${isHoverable ? 'cursor-pointer' : ''} ${className} transition-all duration-300`}
        >
            {Icon && (
                <motion.div
                    className="mb-4"
                    initial={{ scale: 0 }}
                    animate={{ scale: 1 }}
                    transition={{ delay: delay + 0.2, type: 'spring' }}
                >
                    <div className="w-12 h-12 bg-gradient-to-br from-blue-500 to-purple-500 rounded-xl flex items-center justify-center">
                        <Icon className="w-6 h-6 text-white" />
                    </div>
                </motion.div>
            )}

            {title && (
                <motion.h3
                    className="text-xl font-bold text-gray-900 mb-2"
                    initial={{ opacity: 0 }}
                    animate={{ opacity: 1 }}
                    transition={{ delay: delay + 0.1 }}
                >
                    {title}
                </motion.h3>
            )}

            {subtitle && (
                <motion.p
                    className="text-sm text-gray-500 mb-4"
                    initial={{ opacity: 0 }}
                    animate={{ opacity: 1 }}
                    transition={{ delay: delay + 0.15 }}
                >
                    {subtitle}
                </motion.p>
            )}

            <motion.div
                initial={{ opacity: 0 }}
                animate={{ opacity: 1 }}
                transition={{ delay: delay + 0.2 }}
            >
                {children}
            </motion.div>

            {footer && (
                <motion.div
                    className="mt-4 pt-4 border-t border-gray-200"
                    initial={{ opacity: 0 }}
                    animate={{ opacity: 1 }}
                    transition={{ delay: delay + 0.25 }}
                >
                    {footer}
                </motion.div>
            )}
        </motion.div>
    );
};

export default AnimatedCard;

