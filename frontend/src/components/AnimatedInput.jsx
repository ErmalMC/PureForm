import { motion } from 'framer-motion';
import { useState } from 'react';

const AnimatedInput = ({
    label,
    type = 'text',
    value,
    onChange,
    onBlur,
    error = null,
    icon: Icon = null,
    placeholder = '',
    required = false,
    disabled = false,
    className = '',
    ...props
}) => {
    const [isFocused, setIsFocused] = useState(false);

    return (
        <motion.div
            className="w-full"
            initial={{ opacity: 0, y: 10 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.3 }}
        >
            {label && (
                <label className="block text-sm font-semibold text-gray-700 mb-2">
                    {label}
                    {required && <span className="text-red-500 ml-1">*</span>}
                </label>
            )}

            <motion.div
                className="relative"
                animate={isFocused ? { scale: 1.01 } : { scale: 1 }}
                transition={{ duration: 0.2 }}
            >
                {Icon && (
                    <motion.div
                        className="absolute left-4 top-1/2 transform -translate-y-1/2 text-gray-400"
                        animate={isFocused ? { color: '#3b82f6' } : { color: '#9ca3af' }}
                    >
                        <Icon className="w-5 h-5" />
                    </motion.div>
                )}

                <input
                    type={type}
                    value={value}
                    onChange={onChange}
                    onBlur={(e) => {
                        setIsFocused(false);
                        onBlur?.(e);
                    }}
                    onFocus={() => setIsFocused(true)}
                    placeholder={placeholder}
                    disabled={disabled}
                    className={`w-full px-4 py-3 ${Icon ? 'pl-12' : ''} bg-gray-50 border-2 border-gray-300 rounded-xl focus:outline-none focus:ring-0 focus:border-blue-500 focus:bg-white transition-all duration-200 disabled:opacity-50 disabled:cursor-not-allowed ${className} ${
                        error ? 'border-red-500 focus:border-red-500' : ''
                    }`}
                    {...props}
                />

                {isFocused && (
                    <motion.div
                        className="absolute bottom-0 left-0 h-1 bg-gradient-to-r from-blue-600 to-purple-600 rounded-full"
                        initial={{ width: 0 }}
                        animate={{ width: '100%' }}
                        transition={{ duration: 0.3 }}
                    />
                )}
            </motion.div>

            {error && (
                <motion.p
                    className="text-red-500 text-sm mt-2 flex items-center gap-1"
                    initial={{ opacity: 0, y: -5 }}
                    animate={{ opacity: 1, y: 0 }}
                    transition={{ duration: 0.2 }}
                >
                    <svg className="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
                        <path fillRule="evenodd" d="M18.101 12.93a1 1 0 00-1.414-1.414L10 16.586l-6.687-6.687a1 1 0 00-1.414 1.414L8.586 18l-6.687 6.687a1 1 0 001.414 1.414L10 19.414l6.687 6.687a1 1 0 001.414-1.414L11.414 18z" clipRule="evenodd" />
                    </svg>
                    {error}
                </motion.p>
            )}
        </motion.div>
    );
};

export default AnimatedInput;

