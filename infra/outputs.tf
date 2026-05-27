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
