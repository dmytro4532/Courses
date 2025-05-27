import { LockOutlined, UserOutlined, MailOutlined } from '@ant-design/icons';
import { Alert, Button, Card, Form, Input, Typography, Space } from 'antd';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import type { AppDispatch, RootState } from '../../store';
import { registerAdmin } from '../../store/slices/authSlice';

const { Title, Text } = Typography;

const AdminUsers = () => {
  const [form] = Form.useForm();
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();
  const { status, registerError: error } = useSelector((state: RootState) => state.auth);

  const onFinish = async (values: { username: string; email: string; password: string }) => {
    dispatch(registerAdmin(values))
      .unwrap()
      .then(() => navigate('/admin/users'));
  };

  return (
    <div style={{ maxWidth: 400, margin: '24px auto', padding: '0 24px' }}>
      <Card>
        <Space direction="vertical" size="large" style={{ width: '100%' }}>
          <Title level={2} style={{ textAlign: 'center', marginBottom: 0 }}>
            Create Admin Account
          </Title>
          <Text type="secondary" style={{ textAlign: 'center', display: 'block' }}>
            Create a new administrator account
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
            name="register-admin"
            onFinish={onFinish}
            layout="vertical"
            requiredMark={false}
          >
            <Form.Item
              name="username"
              rules={[
                { required: true, message: 'Please enter username' },
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
                { required: true, message: 'Please enter email' },
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
                { required: true, message: 'Please enter password' },
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
                { required: true, message: 'Please confirm password' },
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
                {status === 'loading' ? 'Creating admin account...' : 'Create Admin Account'}
              </Button>
            </Form.Item>
          </Form>
        </Space>
      </Card>
    </div>
  );
};

export default AdminUsers; 