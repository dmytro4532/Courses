import { LockOutlined, UserOutlined, MailOutlined } from '@ant-design/icons';
import { Alert, Button, Card, Form, Input, Typography, Space } from 'antd';
import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Link, useNavigate } from 'react-router-dom';
import type { AppDispatch, RootState } from '../store';
import { register, clearErrors } from '../store/slices/authSlice';

const { Title, Text } = Typography;

const Register = () => {
  const [form] = Form.useForm();
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();
  const { status, registerError: error } = useSelector((state: RootState) => state.auth);

  useEffect(() => {
    dispatch(clearErrors());
  }, [dispatch]);

  const onFinish = async (values: { username: string; email: string; password: string }) => {
    dispatch(register(values))
      .unwrap()
      .then(() => navigate('/login'));
  };

  return (
    <div style={{ maxWidth: 400, margin: '100px auto', padding: '0 24px' }}>
      <Card>
        <Space direction="vertical" size="large" style={{ width: '100%' }}>
          <Title level={2} style={{ textAlign: 'center', marginBottom: 0 }}>
            Create Account
          </Title>
          <Text type="secondary" style={{ textAlign: 'center', display: 'block' }}>
            Sign up to get started
          </Text>

          {error && (
            <Alert
              type="error"
              message={error}
              style={{ marginBottom: 0 }}
              showIcon
              closable
            />
          )}

          <Form
            form={form}
            name="register"
            onFinish={onFinish}
            layout="vertical"
            requiredMark={false}
          >
            <Form.Item
              name="username"
              rules={[
                { required: true, message: 'Please enter your username' },
                { min: 3, message: 'Username must be at least 3 characters' }
              ]}
            >
              <Input
                prefix={<UserOutlined />}
                placeholder="Username"
                size="large"
                disabled={status === 'loading'}
              />
            </Form.Item>

            <Form.Item
              name="email"
              rules={[
                { required: true, message: 'Please enter your email' },
                { type: 'email', message: 'Please enter a valid email' }
              ]}
            >
              <Input
                prefix={<MailOutlined />}
                placeholder="Email"
                size="large"
                disabled={status === 'loading'}
              />
            </Form.Item>

            <Form.Item
              name="password"
              rules={[
                { required: true, message: 'Please enter your password' },
                { min: 6, message: 'Password must be at least 6 characters' }
              ]}
            >
              <Input.Password
                prefix={<LockOutlined />}
                placeholder="Password"
                size="large"
                disabled={status === 'loading'}
              />
            </Form.Item>

            <Form.Item
              name="confirmPassword"
              dependencies={['password']}
              rules={[
                { required: true, message: 'Please confirm your password' },
                ({ getFieldValue }) => ({
                  validator(_, value) {
                    if (!value || getFieldValue('password') === value) {
                      return Promise.resolve();
                    }
                    return Promise.reject(new Error('The two passwords do not match'));
                  },
                }),
              ]}
            >
              <Input.Password
                prefix={<LockOutlined />}
                placeholder="Confirm Password"
                size="large"
                disabled={status === 'loading'}
              />
            </Form.Item>

            <Form.Item>
              <Button
                type="primary"
                htmlType="submit"
                size="large"
                block
                loading={status === 'loading'}
              >
                {status === 'loading' ? 'Creating account...' : 'Create Account'}
              </Button>
            </Form.Item>
          </Form>

          <div style={{ textAlign: 'center' }}>
            <Text type="secondary">
              Already have an account? <Link to="/login">Sign in</Link>
            </Text>
          </div>
        </Space>
      </Card>
    </div>
  );
};

export default Register; 