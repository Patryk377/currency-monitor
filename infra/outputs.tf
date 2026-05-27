output "vpc_id" {
  description = "ID of the VPC"
  value       = module.vpc.vpc_id
}

output "private_subnet_ids" {
  description = "IDs of the private subnets (for EKS nodes and RDS)"
  value       = module.vpc.private_subnets
}

output "public_subnet_ids" {
  description = "IDs of the public subnets (for ALB)"
  value       = module.vpc.public_subnets
}
output "ecr_repository_urls" {
  description = "Map of ECR repository URLs (use these in CI/CD to push images)"
  value       = { for k, repo in aws_ecr_repository.this : k => repo.repository_url }
}
output "rds_endpoint" {
  description = "RDS instance endpoint (host:port)"
  value       = aws_db_instance.this.endpoint
}

output "rds_address" {
  description = "RDS instance hostname (without port) - use this in connection strings"
  value       = aws_db_instance.this.address
}

output "rds_port" {
  description = "RDS instance port"
  value       = aws_db_instance.this.port
}

output "eks_cluster_name" {
  description = "EKS cluster name (use with: aws eks update-kubeconfig)"
  value       = module.eks.cluster_name
}

output "eks_cluster_endpoint" {
  description = "EKS API server endpoint"
  value       = module.eks.cluster_endpoint
}

output "eks_cluster_region" {
  description = "Region where the cluster lives"
  value       = var.aws_region
}
