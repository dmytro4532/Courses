import { Card, Typography, Avatar, Descriptions, Spin, Alert } from 'antd';
import { UserOutlined } from '@ant-design/icons';
import { useSelector, useDispatch } from 'react-redux';
import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import type { RootState, AppDispatch } from '../store';
import { fetchCurrentUser } from '../store/slices/authSlice';

const { Title } = Typography;

const Profile = () => {
    const navigate = useNavigate();
    const dispatch = useDispatch<AppDispatch>();
    const { user, token, status } = useSelector((state: RootState) => state.auth);

    useEffect(() => {
        if (!token) {
            navigate('/login');
            return;
        }

        if (token && !user) {
            dispatch(fetchCurrentUser());
        }
    }, [token, user, navigate, dispatch]);

    if (!token) {
        return null;
    }

    if (status === 'loading') {
        return (
            <div style={{ textAlign: 'center', padding: '50px' }}>
                <Spin size="large" />
            </div>
        );
    }

    return (
        <div style={{ maxWidth: '800px', margin: '0 auto', padding: '24px' }}>
            <Title level={2}>My Profile</Title>
            {user ? (
                <Card>
                    <div style={{ display: 'flex', alignItems: 'center', marginBottom: '24px' }}>
                        <Avatar size={80} icon={<UserOutlined />} />
                        <div style={{ marginLeft: '24px' }}>
                            <Title level={3}>{user.username}</Title>
                            <Typography.Text type="secondary">{user.role}</Typography.Text>
                        </div>
                    </div>
                    <Descriptions bordered column={1}>
                        <Descriptions.Item label="Email">{user.email}</Descriptions.Item>
                        <Descriptions.Item label="User ID">{user.id}</Descriptions.Item>
                        <Descriptions.Item label="Role">{user.role}</Descriptions.Item>
                    </Descriptions>
                </Card>
            ) : (
                <Alert
                    message="User information not available"
                    description="There was a problem loading your profile information. Please try again later."
                    type="warning"
                    showIcon
                />
            )}
        </div>
    );
};

export default Profile; 