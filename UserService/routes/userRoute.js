const express = require('express');
const router = express.Router();

const { authenticateToken } = require('../controllers/authController');
const UserController = require('../controllers/userController');


router.post('/register', UserController.register);
router.post('/login', UserController.login);
router.get('/profile', authenticateToken, UserController.getProfile);

module.exports = router;