const bcrypt = require('bcryptjs');
const jwt = require('jsonwebtoken');
const { pool } = require('../config/database');

const JWT_SECRET = process.env.JWT_SECRET || 'idontknow';

// Register
const register = async (req, res) => {
    try {
        const {username, email, password} = req.body;
        
        // Check if user exists
        const existingUser = await pool.query('SELECT * FROM users WHERE username = $1 OR email = $2', [username, email]);
        if (existingUser.rows.length > 0) {
            return res.status(400).json({ error: 'User with this username or email already exists' });
        }

        // Hash password
        const saltRounds = 10;
        const passwordHash = await bcrypt.hash(password, saltRounds);

        // Insert new user
        const newUser = await pool.query('INSERT INTO users (username, email, password_hash) VALUES ($1, $2, $3) RETURNING id, username, email, created_at', [username, email, passwordHash]);

        // Generate JWT token
        const token = jwt.sign({ userId: newUser.rows[0].id }, JWT_SECRET, { expiresIn: '1h' });

        res.status(201).json({
            message: 'User registered successfully',
            user: newUser.rows[0],
            token
        });
    } catch (error) {
        console.error('Error registering user:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
};

// Login
const login = async (req, res) => {
    try {
        const {username, password} = req.body;
        // Find user by username
        const user = await pool.query('SELECT * FROM users WHERE username = $1', [username]);
        if (user.rows.length === 0) {
            return res.status(401).json({ error: 'User not found' });
        }

        // Check password
        const isValidPassword = await bcrypt.compare(password, user.rows[0].password_hash);
        if (!isValidPassword) {
            return res.status(401).json({ error: 'Invalid credentials' });
        }

        // Generate JWT token
        const token = jwt.sign({ userId: user.rows[0].id }, JWT_SECRET, { expiresIn: '1h' });

        res.status(200).json({
            message: 'Login successful',
            user: user.rows[0],
            token
        });
    } catch (error) {
        console.error('Error logging in:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
};

// Get user profile with history
const getProfile = async (req, res) => {
    try {
        const user = await pool.query('SELECT id, username, email, created_at FROM users WHERE id = $1', [req.user.userId]);
        if (user.rows.length === 0) {
            return res.status(404).json({ error: 'User not found' });
        }
        
        // Get user's URL history
        const history = await pool.query(
            'SELECT long_url, short_url, created_at FROM url_history WHERE user_id = $1 ORDER BY created_at DESC',
            [req.user.userId]
        );
        
        res.status(200).json({
            user: user.rows[0],
            history: history.rows
        });
    } catch (error) {
        console.error('Error getting user profile:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
};

// Add URL to user history
const addToHistory = async (req, res) => {
    try {
        console.log('📝 AddToHistory received data:', JSON.stringify(req.body, null, 2));

        const { userId, longUrl, shortUrl, createdAt } = req.body;

        console.log('📝 Extracted values:', { userId, longUrl, shortUrl, createdAt });

        if (!userId || !longUrl || !shortUrl) {
            console.log('❌ Missing required fields:', { userId: !!userId, longUrl: !!longUrl, shortUrl: !!shortUrl });
            return res.status(400).json({ error: 'Missing required fields' });
        }

        console.log('📝 Inserting into database...');

        // Insert into history table
        const result = await pool.query(
            'INSERT INTO url_history (user_id, long_url, short_url, created_at) VALUES ($1, $2, $3, $4) RETURNING *',
            [userId, longUrl, shortUrl, createdAt || new Date()]
        );

        console.log('✅ Successfully inserted into history:', result.rows[0]);

        res.status(201).json({ message: 'URL added to history successfully' });
    } catch (error) {
        console.error('❌ Error adding URL to history:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
};

module.exports = {
    register,
    login,
    getProfile,
    addToHistory
};