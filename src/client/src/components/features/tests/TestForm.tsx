import { Form, Input, Button, Space, message } from 'antd';
import { useState, useEffect } from 'react';
import type { Test } from '../../../types';
import api from '../../../api/axios';

interface TestFormProps {
  initialValues?: Test;
  onSuccess: () => void;
  onCancel: () => void;
  loading?: boolean;
}

interface TestFormData {
  title: string;
}

export const TestForm = ({ initialValues, onSuccess, onCancel, loading }: TestFormProps) => {
  const [form] = Form.useForm();
  const [submitting, setSubmitting] = useState(false);

  // Reset form when initialValues changes (including when modal opens/closes)
  useEffect(() => {
    form.resetFields();
  }, [form, initialValues]);

  const handleSubmit = async (values: TestFormData) => {
    setSubmitting(true);
    try {
      if (initialValues) {
        await api.put(`/api/tests/${initialValues.id}`, {
          id: initialValues.id,
          ...values,
        });
      } else {
        await api.post('/api/tests', values);
      }

      message.success(`Test ${initialValues ? 'updated' : 'created'} successfully`);
      onSuccess();
    } catch (error: any) {
      message.error(error?.response?.data?.details || `Failed to ${initialValues ? 'update' : 'create'} test`);
    } finally {
      setSubmitting(false);
    }
  };

  const handleCancel = () => {
    form.resetFields();
    onCancel();
  };

  return (
    <Form
      form={form}
      layout="vertical"
      onFinish={handleSubmit}
      initialValues={initialValues}
    >
      <Form.Item
        name="title"
        label="Title"
        rules={[{ required: true, message: 'Please enter the test title' }]}
      >
        <Input />
      </Form.Item>

      <Form.Item>
        <Space>
          <Button
            type="primary"
            htmlType="submit"
            loading={submitting || loading}
          >
            {initialValues ? 'Update' : 'Create'}
          </Button>
          <Button onClick={handleCancel}>
            Cancel
          </Button>
        </Space>
      </Form.Item>
    </Form>
  );
}; 