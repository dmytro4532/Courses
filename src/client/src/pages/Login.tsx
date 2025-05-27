import { LockOutlined, UserOutlined } from '@ant-design/icons';
import { Alert, Button, Card, Form, Input, Typography, Space } from 'antd';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate, Link, useLocation } from 'react-router-dom';
import type { AppDispatch, RootState } from '../store';
import { login } from '../store/slices/authSlice';

const { Title, Text } = Typography;

const Login = () => {
  const [form] = Form.useForm();
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();
  const location = useLocation();
  const { status, registerError: error } = useSelector((state: RootState) => state.auth);

  const from = location.state?.from?.pathname || '/';

  const onFinish = async (values: { email: string; password: string }) => {
    dispatch(login(values))
      .unwrap()
      .then(() => navigate(from, { replace: true }));
  };

  return (
    <div style={{ maxWidth: 400, margin: '100px auto', padding: '0 24px' }}>
      <Card>
        <Space direction="vertical" size="large" style={{ width: '100%' }}>
          <Title level={2} style={{ textAlign: 'center', marginBottom: 0 }}>
            Welcome Back
          </Title>
          <Text type="secondary" style={{ textAlign: 'center', display: 'block' }}>
            Please sign in to continue
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
            name="login"
            onFinish={onFinish}
            layout="vertical"
            requiredMark={false}
          >
            <Form.Item
              name="email"
              rules={[
                { required: true, message: 'Please enter your email' },
                { type: 'email' }
              ]}
            >
              <Input
                prefix={<UserOutlined />}
                placeholder="Email"
                size="large"
                disabled={status === 'loading'}
              />
            </Form.Item>

            <Form.Item
              name="password"
              rules={[
                { required: true, message: 'Please enter your password' },
              ]}
            >
              <Input.Password
                prefix={<LockOutlined />}
                placeholder="Password"
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
                {status === 'loading' ? 'Signing in...' : 'Sign in'}
              </Button>
            </Form.Item>
          </Form>

          <div style={{ textAlign: 'center' }}>
            <Text type="secondary">
              Don't have an account? <Link to="/register">Sign up</Link>
            </Text>
          </div>
        </Space>
      </Card>
    </div>
  );
};

export default Login; 